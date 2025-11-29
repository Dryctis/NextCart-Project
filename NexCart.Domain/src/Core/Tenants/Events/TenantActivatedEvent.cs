using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Tenants.Events;

public sealed class TenantActivatedEvent : DomainEventBase
{
    public TenantId TenantId { get; }

    public TenantActivatedEvent(TenantId tenantId)
    {
        TenantId = tenantId;
    }
}