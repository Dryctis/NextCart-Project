namespace NexCart.Application.Common.Interfaces;

public interface ITenantService
{
   
    Guid? TenantId { get; }

    string? TenantSlug { get; }

    bool HasTenant { get; }


    void SetTenant(Guid tenantId, string tenantSlug);
}