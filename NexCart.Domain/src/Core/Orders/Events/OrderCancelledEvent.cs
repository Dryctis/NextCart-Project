using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Orders.Events;

public sealed class OrderCancelledEvent : DomainEventBase
{
    public OrderId OrderId { get; }
    public string Reason { get; }

    public OrderCancelledEvent(OrderId orderId, string reason)
    {
        OrderId = orderId;
        Reason = reason;
    }
}