using NexCart.Domain.Common.Entities;
using NexCart.Domain.Common.ValueObjects;
using NexCart.Domain.Tenants.Events;

namespace NexCart.Domain.Tenants;

public sealed class Tenant : AggregateRoot<TenantId>, IAuditableEntity
{
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public BusinessVertical Vertical { get; private set; }
    public TenantStatus Status { get; private set; }
    public TenantSettings Settings { get; private set; }

    public Email OwnerEmail { get; private set; }
    public string OwnerName { get; private set; }

    public SubscriptionPlan Plan { get; private set; }
    public DateTime? TrialEndsAt { get; private set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    private Tenant() : base()
    {
        Name = string.Empty;
        Slug = string.Empty;
        OwnerEmail = null!;
        OwnerName = string.Empty;
        Settings = null!;
        CreatedBy = string.Empty;
    }

    private Tenant(
        TenantId id,
        string name,
        string slug,
        Email ownerEmail,
        string ownerName,
        BusinessVertical vertical)
        : base(id)
    {
        Name = name;
        Slug = slug;
        OwnerEmail = ownerEmail;
        OwnerName = ownerName;
        Vertical = vertical;
        Status = TenantStatus.Trial;
        Plan = SubscriptionPlan.Free;
        TrialEndsAt = DateTime.UtcNow.AddDays(30);
        Settings = TenantSettings.CreateDefault(vertical);
        CreatedBy = string.Empty;

        AddDomainEvent(new TenantCreatedEvent(id, name, vertical));
    }

    public static Tenant Create(
        string name,
        string slug,
        string ownerEmail,
        string ownerName,
        BusinessVertical vertical)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del tenant es requerido", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("El nombre del tenant es muy largo", nameof(name));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("El slug del tenant es requerido", nameof(slug));

        if (!IsValidSlug(slug))
            throw new ArgumentException(
                "El slug debe contener solo letras minúsculas, números y guiones",
                nameof(slug));

        if (string.IsNullOrWhiteSpace(ownerName))
            throw new ArgumentException("El nombre del propietario es requerido", nameof(ownerName));

        var email = Email.Create(ownerEmail);

        return new Tenant(
            TenantId.CreateUnique(),
            name.Trim(),
            slug.ToLowerInvariant(),
            email,
            ownerName.Trim(),
            vertical);
    }

    public void Activate()
    {
        if (Status == TenantStatus.Active)
            throw new InvalidOperationException("El tenant ya está activo");

        Status = TenantStatus.Active;
        AddDomainEvent(new TenantActivatedEvent(Id));
    }

    public void Suspend(string reason)
    {
        if (Status == TenantStatus.Suspended)
            throw new InvalidOperationException("El tenant ya está suspendido");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("La razón de suspensión es requerida", nameof(reason));

        Status = TenantStatus.Suspended;
        AddDomainEvent(new TenantSuspendedEvent(Id, reason));
    }

    public void Cancel()
    {
        if (Status == TenantStatus.Cancelled)
            throw new InvalidOperationException("El tenant ya está cancelado");

        Status = TenantStatus.Cancelled;
    }

    public void UpdateSettings(TenantSettings newSettings)
    {
        Settings = newSettings ?? throw new ArgumentNullException(nameof(newSettings));
        AddDomainEvent(new TenantSettingsUpdatedEvent(Id));
    }

    public void UpgradePlan(SubscriptionPlan newPlan)
    {
        if (newPlan <= Plan)
            throw new InvalidOperationException("El nuevo plan debe ser superior al actual");

        Plan = newPlan;
        TrialEndsAt = null;
    }

    public void DowngradePlan(SubscriptionPlan newPlan)
    {
        if (newPlan >= Plan)
            throw new InvalidOperationException("El nuevo plan debe ser inferior al actual");

        Plan = newPlan;
    }

    public bool IsTrialExpired()
    {
        return Status == TenantStatus.Trial &&
               TrialEndsAt.HasValue &&
               TrialEndsAt.Value < DateTime.UtcNow;
    }

    private static bool IsValidSlug(string slug)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(
            slug,
            @"^[a-z0-9]+(?:-[a-z0-9]+)*$");
    }
}