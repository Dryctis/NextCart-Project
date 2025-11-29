using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Tenants.Events;

public sealed class TenantSettingsUpdatedEvent : DomainEventBase
{
    public TenantId TenantId { get; }

    public TenantSettingsUpdatedEvent(TenantId tenantId)
    {
        TenantId = tenantId;
    }
}