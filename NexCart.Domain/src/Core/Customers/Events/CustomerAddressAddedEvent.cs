using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Customers.Events;

public sealed class CustomerAddressAddedEvent : DomainEventBase
{
    public CustomerId CustomerId { get; }
    public CustomerAddressId AddressId { get; }

    public CustomerAddressAddedEvent(CustomerId customerId, CustomerAddressId addressId)
    {
        CustomerId = customerId;
        AddressId = addressId;
    }
}