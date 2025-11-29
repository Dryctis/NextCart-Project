namespace NexCart.Domain.Common.Events;


public abstract class DomainEventBase : IDomainEvent
{
    public DateTime OccurredOn { get; }

    protected DomainEventBase()
    {
        OccurredOn = DateTime.UtcNow;
    }
}