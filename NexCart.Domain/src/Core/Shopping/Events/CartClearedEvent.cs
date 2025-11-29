using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Shopping.Events;

public sealed class CartClearedEvent : DomainEventBase
{
    public ShoppingCartId CartId { get; }

    public CartClearedEvent(ShoppingCartId cartId)
    {
        CartId = cartId;
    }
}