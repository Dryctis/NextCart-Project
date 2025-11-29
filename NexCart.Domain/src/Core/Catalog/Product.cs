using NexCart.Domain.Catalog.Enums;
using NexCart.Domain.Catalog.Events;
using NexCart.Domain.Catalog.ValueObjects;
using NexCart.Domain.Common.Entities;
using NexCart.Domain.Common.ValueObjects;
using NexCart.Domain.Tenants;

namespace NexCart.Domain.Catalog;

public sealed class Product : AggregateRoot<ProductId>, IAuditableEntity, ISoftDeletable
{
    public TenantId TenantId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Sku Sku { get; private set; }
    public Money Price { get; private set; }
    public Money? CompareAtPrice { get; private set; }
    public Money Cost { get; private set; }
    public ProductType Type { get; private set; }
    public ProductStatus Status { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public BrandId? BrandId { get; private set; }
    public int StockQuantity { get; private set; }
    public int LowStockThreshold { get; private set; }
    public bool TrackInventory { get; private set; }
    public Weight? Weight { get; private set; }
    public Dimensions? Dimensions { get; private set; }
    public Rating Rating { get; private set; }
    public bool IsFeatured { get; private set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    private Product() : base()
    {
        TenantId = null!;
        Name = string.Empty;
        Description = string.Empty;
        Sku = null!;
        Price = null!;
        Cost = null!;
        CategoryId = null!;
        Rating = Rating.Empty();
        CreatedBy = string.Empty;
    }

    private Product(
        ProductId id,
        TenantId tenantId,
        string name,
        string description,
        Sku sku,
        Money price,
        Money cost,
        ProductType type,
        CategoryId categoryId)
        : base(id)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        Sku = sku;
        Price = price;
        Cost = cost;
        Type = type;
        Status = ProductStatus.Draft;
        CategoryId = categoryId;
        StockQuantity = 0;
        LowStockThreshold = 10;
        TrackInventory = true;
        Rating = Rating.Empty();
        IsFeatured = false;
        CreatedBy = string.Empty;

        AddDomainEvent(new ProductCreatedEvent(id, name));
    }

    public static Product Create(
        TenantId tenantId,
        string name,
        string description,
        string sku,
        decimal price,
        decimal cost,
        ProductType type,
        CategoryId categoryId,
        string currency = "USD")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del producto es requerido", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("El nombre del producto es muy largo", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción del producto es requerida", nameof(description));

        var skuValueObject = Sku.Create(sku);
        var priceValueObject = Money.Of(price, currency);
        var costValueObject = Money.Of(cost, currency);

        if (cost > price)
            throw new ArgumentException("El costo no puede ser mayor al precio de venta");

        return new Product(
            ProductId.CreateUnique(),
            tenantId,
            name.Trim(),
            description.Trim(),
            skuValueObject,
            priceValueObject,
            costValueObject,
            type,
            categoryId);
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice <= Money.Zero(Price.Currency))
            throw new ArgumentException("El precio debe ser mayor a cero");

        if (newPrice < Cost)
            throw new ArgumentException("El precio no puede ser menor al costo");

        var oldPrice = Price;
        Price = newPrice;

        AddDomainEvent(new ProductPriceChangedEvent(Id, oldPrice, newPrice));
    }

    public void SetCompareAtPrice(Money compareAtPrice)
    {
        if (compareAtPrice <= Price)
            throw new ArgumentException("El precio de comparación debe ser mayor al precio actual");

        CompareAtPrice = compareAtPrice;
    }

    public void RemoveCompareAtPrice()
    {
        CompareAtPrice = null;
    }

    public void UpdateStock(int quantity)
    {
        if (!TrackInventory)
            throw new InvalidOperationException("Este producto no rastrea inventario");

        if (quantity < 0)
            throw new ArgumentException("La cantidad no puede ser negativa", nameof(quantity));

        var oldQuantity = StockQuantity;
        StockQuantity = quantity;

        AddDomainEvent(new InventoryUpdatedEvent(Id, oldQuantity, quantity));

        UpdateStatus();
    }

    public void ReduceStock(int quantity)
    {
        if (!TrackInventory)
            return;

        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(quantity));

        if (StockQuantity < quantity)
            throw new InvalidOperationException("Stock insuficiente");

        UpdateStock(StockQuantity - quantity);
    }

    public void IncreaseStock(int quantity)
    {
        if (!TrackInventory)
            return;

        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(quantity));

        UpdateStock(StockQuantity + quantity);
    }

    public void Activate()
    {
        if (Status == ProductStatus.Active)
            throw new InvalidOperationException("El producto ya está activo");

        Status = ProductStatus.Active;
        UpdateStatus();
    }

    public void Deactivate()
    {
        if (Status == ProductStatus.Inactive)
            throw new InvalidOperationException("El producto ya está inactivo");

        Status = ProductStatus.Inactive;
    }

    public void Discontinue()
    {
        Status = ProductStatus.Discontinued;
    }

    public void SetFeatured(bool isFeatured)
    {
        IsFeatured = isFeatured;
    }

    public void SetWeight(Weight weight)
    {
        Weight = weight;
    }

    public void SetDimensions(Dimensions dimensions)
    {
        Dimensions = dimensions;
    }

    public void SetBrand(BrandId brandId)
    {
        BrandId = brandId;
    }

    public void UpdateCategory(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }

    public StockStatus GetStockStatus()
    {
        if (!TrackInventory)
            return StockStatus.InStock;

        if (StockQuantity == 0)
            return StockStatus.OutOfStock;

        if (StockQuantity <= LowStockThreshold)
            return StockStatus.LowStock;

        return StockStatus.InStock;
    }

    public bool IsInStock()
    {
        return !TrackInventory || StockQuantity > 0;
    }

    public decimal CalculateProfit()
    {
        return Price.Amount - Cost.Amount;
    }

    public decimal CalculateProfitMargin()
    {
        if (Price.Amount == 0)
            return 0;

        return ((Price.Amount - Cost.Amount) / Price.Amount) * 100;
    }

    private void UpdateStatus()
    {
        if (Status == ProductStatus.Discontinued)
            return;

        if (TrackInventory && StockQuantity == 0)
            Status = ProductStatus.OutOfStock;
        else if (Status == ProductStatus.OutOfStock && StockQuantity > 0)
            Status = ProductStatus.Active;
    }
}