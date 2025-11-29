using NexCart.Domain.Catalog;
using NexCart.Domain.Common.Entities;
using NexCart.Domain.Common.ValueObjects;
using NexCart.Domain.Shopping.Enums;
using NexCart.Domain.Shopping.Events;
using NexCart.Domain.Tenants;

namespace NexCart.Domain.Shopping;

public sealed class ShoppingCart : AggregateRoot<ShoppingCartId>
{
    public TenantId TenantId { get; private set; }
    public string? CustomerId { get; private set; }
    public string? CustomerEmail { get; private set; }
    public string SessionId { get; private set; }
    public CartStatus Status { get; private set; }
    public Money Total { get; private set; }
    public int TotalItems { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? AbandonedAt { get; private set; }
    public DateTime? ConvertedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    private ShoppingCart() : base()
    {
        TenantId = null!;
        SessionId = string.Empty;
        Total = null!;
    }

    private ShoppingCart(
        ShoppingCartId id,
        TenantId tenantId,
        string sessionId,
        string? customerId,
        string? customerEmail)
        : base(id)
    {
        TenantId = tenantId;
        SessionId = sessionId;
        CustomerId = customerId;
        CustomerEmail = customerEmail;
        Status = CartStatus.Active;
        Total = Money.Zero();
        TotalItems = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddDays(30);
    }

    public static ShoppingCart Create(
        TenantId tenantId,
        string sessionId,
        string? customerId = null,
        string? customerEmail = null)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
            throw new ArgumentException("El ID de sesión es requerido", nameof(sessionId));

        return new ShoppingCart(
            ShoppingCartId.CreateUnique(),
            tenantId,
            sessionId.Trim(),
            customerId?.Trim(),
            customerEmail?.Trim());
    }

    public void AddItem(
        ProductId productId,
        ProductVariantId? productVariantId,
        string productName,
        string? variantName,
        Money unitPrice,
        int quantity,
        string? imageUrl = null)
    {
        if (Status != CartStatus.Active)
            throw new InvalidOperationException("No se pueden agregar items a un carrito inactivo");

        if (IsExpired())
            throw new InvalidOperationException("El carrito ha expirado");

        var existingItem = FindItem(productId, productVariantId);

        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            var newItem = CartItem.Create(
                Id,
                productId,
                productVariantId,
                productName,
                variantName,
                unitPrice,
                quantity,
                imageUrl);

            _items.Add(newItem);
        }

        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ItemAddedToCartEvent(Id, productId, quantity));
    }

    public void UpdateItemQuantity(CartItemId itemId, int newQuantity)
    {
        if (Status != CartStatus.Active)
            throw new InvalidOperationException("No se pueden modificar items en un carrito inactivo");

        var item = _items.FirstOrDefault(i => i.Id.Equals(itemId));

        if (item == null)
            throw new InvalidOperationException("Item no encontrado en el carrito");

        item.UpdateQuantity(newQuantity);
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(CartItemId itemId)
    {
        if (Status != CartStatus.Active)
            throw new InvalidOperationException("No se pueden eliminar items de un carrito inactivo");

        var item = _items.FirstOrDefault(i => i.Id.Equals(itemId));

        if (item == null)
            throw new InvalidOperationException("Item no encontrado en el carrito");

        _items.Remove(item);
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ItemRemovedFromCartEvent(Id, item.ProductId));
    }

    public void RemoveItemByProduct(ProductId productId, ProductVariantId? productVariantId = null)
    {
        var item = FindItem(productId, productVariantId);

        if (item != null)
        {
            RemoveItem(item.Id);
        }
    }

    public void Clear()
    {
        if (_items.Count == 0)
            return;

        _items.Clear();
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CartClearedEvent(Id));
    }

    public void AssignToCustomer(string customerId, string? customerEmail)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentException("El ID del cliente es requerido", nameof(customerId));

        CustomerId = customerId.Trim();
        CustomerEmail = customerEmail?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsAbandoned()
    {
        if (Status == CartStatus.Abandoned)
            return;

        Status = CartStatus.Abandoned;
        AbandonedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CartAbandonedEvent(Id, CustomerEmail));
    }

    public void MarkAsConverted()
    {
        if (Status == CartStatus.Converted)
            return;

        Status = CartStatus.Converted;
        ConvertedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsExpired()
    {
        Status = CartStatus.Expired;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ExtendExpiration(int days)
    {
        if (days <= 0)
            throw new ArgumentException("Los días deben ser mayores a cero", nameof(days));

        ExpiresAt = DateTime.UtcNow.AddDays(days);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsEmpty() => _items.Count == 0;

    public bool HasItems() => _items.Count > 0;

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    public bool ContainsProduct(ProductId productId, ProductVariantId? productVariantId = null)
    {
        return FindItem(productId, productVariantId) != null;
    }

    public int GetItemCount(ProductId productId, ProductVariantId? productVariantId = null)
    {
        var item = FindItem(productId, productVariantId);
        return item?.Quantity ?? 0;
    }

    private CartItem? FindItem(ProductId productId, ProductVariantId? productVariantId)
    {
        return _items.FirstOrDefault(i =>
            i.ProductId.Equals(productId) &&
            (productVariantId == null
                ? i.ProductVariantId == null
                : i.ProductVariantId != null && i.ProductVariantId.Equals(productVariantId)));
    }

    private void RecalculateTotal()
    {
        if (_items.Count == 0)
        {
            Total = Money.Zero();
            TotalItems = 0;
            return;
        }

        var firstItem = _items.First();
        var currency = firstItem.UnitPrice.Currency;

        var total = Money.Zero(currency);

        foreach (var item in _items)
        {
            total = total.Add(item.Subtotal);
        }

        Total = total;
        TotalItems = _items.Sum(i => i.Quantity);
    }
}