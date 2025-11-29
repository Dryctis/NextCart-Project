using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Tenants.Events;

public sealed class TenantCreatedEvent : DomainEventBase
{
    public TenantId TenantId { get; }
    public string Name { get; }
    public BusinessVertical Vertical { get; }

    public TenantCreatedEvent(TenantId tenantId, string name, BusinessVertical vertical)
    {
        TenantId = tenantId;
        Name = name;
        Vertical = vertical;
    }
}