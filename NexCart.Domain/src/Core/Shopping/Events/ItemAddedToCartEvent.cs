using NexCart.Domain.Catalog;
using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Shopping.Events;

public sealed class ItemAddedToCartEvent : DomainEventBase
{
    public ShoppingCartId CartId { get; }
    public ProductId ProductId { get; }
    public int Quantity { get; }

    public ItemAddedToCartEvent(ShoppingCartId cartId, ProductId productId, int quantity)
    {
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
    }
}