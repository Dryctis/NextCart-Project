using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Tenants.Events;

public sealed class TenantSuspendedEvent : DomainEventBase
{
    public TenantId TenantId { get; }
    public string Reason { get; }

    public TenantSuspendedEvent(TenantId tenantId, string reason)
    {
        TenantId = tenantId;
        Reason = reason;
    }
}