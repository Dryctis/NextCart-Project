using NexCart.Domain.Common.Entities;
using NexCart.Domain.Tenants;

namespace NexCart.Domain.Catalog;

public sealed class Category : Entity<CategoryId>, IAuditableEntity, ISoftDeletable
{
    public TenantId TenantId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Slug { get; private set; }
    public CategoryId? ParentCategoryId { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }
    public string? ImageUrl { get; private set; }
    public string? IconUrl { get; private set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    private Category() : base()
    {
        TenantId = null!;
        Name = string.Empty;
        Description = string.Empty;
        Slug = string.Empty;
        CreatedBy = string.Empty;
    }

    private Category(
        CategoryId id,
        TenantId tenantId,
        string name,
        string description,
        string slug,
        CategoryId? parentCategoryId = null)
        : base(id)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        Slug = slug;
        ParentCategoryId = parentCategoryId;
        DisplayOrder = 0;
        IsActive = true;
        CreatedBy = string.Empty;
    }

    public static Category Create(
        TenantId tenantId,
        string name,
        string description,
        string slug,
        CategoryId? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la categoría es requerido", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("El nombre de la categoría es muy largo", nameof(name));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("El slug de la categoría es requerido", nameof(slug));

        if (!IsValidSlug(slug))
            throw new ArgumentException(
                "El slug debe contener solo letras minúsculas, números y guiones",
                nameof(slug));

        return new Category(
            CategoryId.CreateUnique(),
            tenantId,
            name.Trim(),
            description?.Trim() ?? string.Empty,
            slug.ToLowerInvariant(),
            parentCategoryId);
    }

    public void UpdateDetails(string name, string description, string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la categoría es requerido", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("El nombre de la categoría es muy largo", nameof(name));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("El slug de la categoría es requerido", nameof(slug));

        if (!IsValidSlug(slug))
            throw new ArgumentException(
                "El slug debe contener solo letras minúsculas, números y guiones",
                nameof(slug));

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Slug = slug.ToLowerInvariant();
    }

    public void SetParentCategory(CategoryId? parentCategoryId)
    {
        if (parentCategoryId != null && parentCategoryId.Equals(Id))
            throw new InvalidOperationException("Una categoría no puede ser su propia categoría padre");

        ParentCategoryId = parentCategoryId;
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

    public void SetImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("La URL de la imagen no puede estar vacía", nameof(imageUrl));

        ImageUrl = imageUrl;
    }

    public void SetIcon(string iconUrl)
    {
        if (string.IsNullOrWhiteSpace(iconUrl))
            throw new ArgumentException("La URL del ícono no puede estar vacía", nameof(iconUrl));

        IconUrl = iconUrl;
    }

    public bool IsRootCategory() => ParentCategoryId == null;

    public bool IsSubcategory() => ParentCategoryId != null;

    private static bool IsValidSlug(string slug)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(
            slug,
            @"^[a-z0-9]+(?:-[a-z0-9]+)*$");
    }
}