using NexCart.Application.Common.Interfaces;

namespace NexCart.Infrastructure.Identity;

public sealed class TenantService : ITenantService
{
    private Guid? _tenantId;
    private string? _tenantSlug;

    public Guid? TenantId => _tenantId;

    public string? TenantSlug => _tenantSlug;

    public bool HasTenant => _tenantId.HasValue;

    public void SetTenant(Guid tenantId, string tenantSlug)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("El ID del tenant no puede estar vacío", nameof(tenantId));

        if (string.IsNullOrWhiteSpace(tenantSlug))
            throw new ArgumentException("El slug del tenant no puede estar vacío", nameof(tenantSlug));

        _tenantId = tenantId;
        _tenantSlug = tenantSlug.Trim().ToLowerInvariant();
    }
}