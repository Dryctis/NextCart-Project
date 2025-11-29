using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Shopping.Events;

public sealed class CartAbandonedEvent : DomainEventBase
{
    public ShoppingCartId CartId { get; }
    public string? CustomerEmail { get; }

    public CartAbandonedEvent(ShoppingCartId cartId, string? customerEmail)
    {
        CartId = cartId;
        CustomerEmail = customerEmail;
    }
}