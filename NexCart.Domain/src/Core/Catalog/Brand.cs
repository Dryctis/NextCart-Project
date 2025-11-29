using NexCart.Domain.Common.Entities;
using NexCart.Domain.Tenants;

namespace NexCart.Domain.Catalog;

public sealed class Brand : Entity<BrandId>, IAuditableEntity, ISoftDeletable
{
    public TenantId TenantId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Slug { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public bool IsActive { get; private set; }
    public int DisplayOrder { get; private set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    private Brand() : base()
    {
        TenantId = null!;
        Name = string.Empty;
        Description = string.Empty;
        Slug = string.Empty;
        CreatedBy = string.Empty;
    }

    private Brand(
        BrandId id,
        TenantId tenantId,
        string name,
        string description,
        string slug)
        : base(id)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        Slug = slug;
        IsActive = true;
        DisplayOrder = 0;
        CreatedBy = string.Empty;
    }

    public static Brand Create(
        TenantId tenantId,
        string name,
        string description,
        string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la marca es requerido", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("El nombre de la marca es muy largo", nameof(name));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("El slug de la marca es requerido", nameof(slug));

        if (!IsValidSlug(slug))
            throw new ArgumentException(
                "El slug debe contener solo letras minúsculas, números y guiones",
                nameof(slug));

        return new Brand(
            BrandId.CreateUnique(),
            tenantId,
            name.Trim(),
            description?.Trim() ?? string.Empty,
            slug.ToLowerInvariant());
    }

    public void UpdateDetails(string name, string description, string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la marca es requerido", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("El nombre de la marca es muy largo", nameof(name));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("El slug de la marca es requerido", nameof(slug));

        if (!IsValidSlug(slug))
            throw new ArgumentException(
                "El slug debe contener solo letras minúsculas, números y guiones",
                nameof(slug));

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Slug = slug.ToLowerInvariant();
    }

    public void SetLogo(string logoUrl)
    {
        if (string.IsNullOrWhiteSpace(logoUrl))
            throw new ArgumentException("La URL del logo no puede estar vacía", nameof(logoUrl));

        LogoUrl = logoUrl;
    }

    public void SetWebsite(string websiteUrl)
    {
        if (string.IsNullOrWhiteSpace(websiteUrl))
            throw new ArgumentException("La URL del sitio web no puede estar vacía", nameof(websiteUrl));

        if (!Uri.TryCreate(websiteUrl, UriKind.Absolute, out _))
            throw new ArgumentException("La URL del sitio web no es válida", nameof(websiteUrl));

        WebsiteUrl = websiteUrl;
    }

    public void SetDisplayOrder(int order)
    {
        if (order < 0)
            throw new ArgumentException("El orden de visualización no puede ser negativo", nameof(order));

        DisplayOrder = order;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private static bool IsValidSlug(string slug)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(
            slug,
            @"^[a-z0-9]+(?:-[a-z0-9]+)*$");
    }
}