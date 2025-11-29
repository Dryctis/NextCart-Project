using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Orders.Events;

public sealed class OrderShippedEvent : DomainEventBase
{
    public OrderId OrderId { get; }
    public string? TrackingNumber { get; }

    public OrderShippedEvent(OrderId orderId, string? trackingNumber)
    {
        OrderId = orderId;
        TrackingNumber = trackingNumber;
    }


}