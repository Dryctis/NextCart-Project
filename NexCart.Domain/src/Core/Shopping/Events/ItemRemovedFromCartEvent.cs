using NexCart.Domain.Catalog;
using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Shopping.Events;

public sealed class ItemRemovedFromCartEvent : DomainEventBase
{
    public ShoppingCartId CartId { get; }
    public ProductId ProductId { get; }

    public ItemRemovedFromCartEvent(ShoppingCartId cartId, ProductId productId)
    {
        CartId = cartId;
        ProductId = productId;
    }
}