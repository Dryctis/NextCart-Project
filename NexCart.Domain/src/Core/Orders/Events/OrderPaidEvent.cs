using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Orders.Events;

public sealed class OrderPaidEvent : DomainEventBase
{
    public OrderId OrderId { get; }
    public string PaymentMethod { get; }

    public OrderPaidEvent(OrderId orderId, string paymentMethod)
    {
        OrderId = orderId;
        PaymentMethod = paymentMethod;
    }
}