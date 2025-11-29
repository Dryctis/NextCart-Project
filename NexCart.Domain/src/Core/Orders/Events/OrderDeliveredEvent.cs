using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Orders.Events;

public sealed class OrderDeliveredEvent : DomainEventBase
{
    public OrderId OrderId { get; }

    public OrderDeliveredEvent(OrderId orderId)
    {
        OrderId = orderId;
    }
}