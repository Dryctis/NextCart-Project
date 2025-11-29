using NexCart.Domain.Common.Events;
using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Orders.Events;

public sealed class OrderPlacedEvent : DomainEventBase
{
    public OrderId OrderId { get; }
    public string OrderNumber { get; }
    public string CustomerId { get; }
    public Money TotalAmount { get; }

    public OrderPlacedEvent(OrderId orderId, string orderNumber, string customerId, Money totalAmount)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        TotalAmount = totalAmount;
    }
}