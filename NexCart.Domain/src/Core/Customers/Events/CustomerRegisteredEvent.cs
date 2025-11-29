using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Customers.Events;

public sealed class CustomerRegisteredEvent : DomainEventBase
{
    public CustomerId CustomerId { get; }
    public string Email { get; }
    public string FullName { get; }

    public CustomerRegisteredEvent(CustomerId customerId, string email, string fullName)
    {
        CustomerId = customerId;
        Email = email;
        FullName = fullName;
    }
}