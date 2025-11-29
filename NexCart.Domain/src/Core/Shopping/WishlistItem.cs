using NexCart.Domain.Catalog;
using NexCart.Domain.Common.Entities;

namespace NexCart.Domain.Shopping;

public sealed class WishlistItem : Entity<WishlistItemId>
{
    public WishlistId WishlistId { get; private set; }
    public ProductId ProductId { get; private set; }
    public ProductVariantId? ProductVariantId { get; private set; }
    public DateTime AddedAt { get; private set; }

    private WishlistItem() : base()
    {
        WishlistId = null!;
        ProductId = null!;
    }

    private WishlistItem(
        WishlistItemId id,
        WishlistId wishlistId,
        ProductId productId,
        ProductVariantId? productVariantId)
        : base(id)
    {
        WishlistId = wishlistId;
        ProductId = productId;
        ProductVariantId = productVariantId;
        AddedAt = DateTime.UtcNow;
    }

    public static WishlistItem Create(
        WishlistId wishlistId,
        ProductId productId,
        ProductVariantId? productVariantId = null)
    {
        return new WishlistItem(
            WishlistItemId.CreateUnique(),
            wishlistId,
            productId,
            productVariantId);
    }
}