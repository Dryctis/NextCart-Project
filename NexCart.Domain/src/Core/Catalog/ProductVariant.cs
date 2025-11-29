using NexCart.Domain.Catalog.ValueObjects;
using NexCart.Domain.Common.Entities;
using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog;

public sealed class ProductVariant : Entity<ProductVariantId>
{
    public ProductId ProductId { get; private set; }
    public string Name { get; private set; }
    public Sku Sku { get; private set; }
    public Money Price { get; private set; }
    public Money? CompareAtPrice { get; private set; }
    public int StockQuantity { get; private set; }
    public Weight? Weight { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; }
    public int DisplayOrder { get; private set; }

    private readonly Dictionary<string, string> _attributes = new();
    public IReadOnlyDictionary<string, string> Attributes => _attributes;

    private ProductVariant() : base()
    {
        ProductId = null!;
        Name = string.Empty;
        Sku = null!;
        Price = null!;
    }

    private ProductVariant(
        ProductVariantId id,
        ProductId productId,
        string name,
        Sku sku,
        Money price)
        : base(id)
    {
        ProductId = productId;
        Name = name;
        Sku = sku;
        Price = price;
        StockQuantity = 0;
        IsActive = true;
        DisplayOrder = 0;
    }

    public static ProductVariant Create(
        ProductId productId,
        string name,
        string sku,
        decimal price,
        string currency = "USD")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la variante es requerido", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("El nombre de la variante es muy largo", nameof(name));

        var skuValueObject = Sku.Create(sku);
        var priceValueObject = Money.Of(price, currency);

        return new ProductVariant(
            ProductVariantId.CreateUnique(),
            productId,
            name.Trim(),
            skuValueObject,
            priceValueObject);
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice <= Money.Zero(Price.Currency))
            throw new ArgumentException("El precio debe ser mayor a cero");

        Price = newPrice;
    }

    public void SetCompareAtPrice(Money compareAtPrice)
    {
        if (compareAtPrice <= Price)
            throw new ArgumentException("El precio de comparación debe ser mayor al precio actual");

        CompareAtPrice = compareAtPrice;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("La cantidad no puede ser negativa", nameof(quantity));

        StockQuantity = quantity;
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(quantity));

        if (StockQuantity < quantity)
            throw new InvalidOperationException("Stock insuficiente");

        StockQuantity -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(quantity));

        StockQuantity += quantity;
    }

    public void AddAttribute(string name, string value)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del atributo es requerido", nameof(name));

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El valor del atributo es requerido", nameof(value));

        _attributes[name.Trim()] = value.Trim();
    }

    public void RemoveAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del atributo es requerido", nameof(name));

        _attributes.Remove(name.Trim());
    }

    public void SetWeight(Weight weight)
    {
        Weight = weight;
    }

    public void SetImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("La URL de la imagen no puede estar vacía", nameof(imageUrl));

        ImageUrl = imageUrl;
    }

    public void SetDisplayOrder(int order)
    {
        if (order < 0)
            throw new ArgumentException("El orden de visualización no puede ser negativo", nameof(order));

        DisplayOrder = order;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public bool IsInStock() => StockQuantity > 0;
}