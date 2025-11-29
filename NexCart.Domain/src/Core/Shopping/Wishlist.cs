using NexCart.Domain.Catalog;
using NexCart.Domain.Common.Entities;
using NexCart.Domain.Tenants;

namespace NexCart.Domain.Shopping;

public sealed class Wishlist : AggregateRoot<WishlistId>
{
    public TenantId TenantId { get; private set; }
    public string CustomerId { get; private set; }
    public string? Name { get; private set; }
    public bool IsPublic { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<WishlistItem> _items = new();
    public IReadOnlyCollection<WishlistItem> Items => _items.AsReadOnly();

    private Wishlist() : base()
    {
        TenantId = null!;
        CustomerId = string.Empty;
    }

    private Wishlist(
        WishlistId id,
        TenantId tenantId,
        string customerId,
        string? name,
        bool isPublic)
        : base(id)
    {
        TenantId = tenantId;
        CustomerId = customerId;
        Name = name;
        IsPublic = isPublic;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Wishlist Create(
        TenantId tenantId,
        string customerId,
        string? name = null,
        bool isPublic = false)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentException("El ID del cliente es requerido", nameof(customerId));

        return new Wishlist(
            WishlistId.CreateUnique(),
            tenantId,
            customerId.Trim(),
            name?.Trim(),
            isPublic);
    }

    public void AddItem(ProductId productId, ProductVariantId? productVariantId = null)
    {
        if (ContainsProduct(productId, productVariantId))
            throw new InvalidOperationException("El producto ya está en la lista de deseos");

        var item = WishlistItem.Create(Id, productId, productVariantId);
        _items.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(WishlistItemId itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id.Equals(itemId));

        if (item == null)
            throw new InvalidOperationException("Item no encontrado en la lista de deseos");

        _items.Remove(item);
        UpdatedAt = DateTime.UtcNow;
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
        _items.Clear();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateName(string name)
    {
        Name = name?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void MakePublic()
    {
        IsPublic = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MakePrivate()
    {
        IsPublic = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool ContainsProduct(ProductId productId, ProductVariantId? productVariantId = null)
    {
        return FindItem(productId, productVariantId) != null;
    }

    public bool IsEmpty() => _items.Count == 0;

    public int Count => _items.Count;

    private WishlistItem? FindItem(ProductId productId, ProductVariantId? productVariantId)
    {
        return _items.FirstOrDefault(i =>
            i.ProductId.Equals(productId) &&
            (productVariantId == null
                ? i.ProductVariantId == null
                : i.ProductVariantId != null && i.ProductVariantId.Equals(productVariantId)));
    }
}