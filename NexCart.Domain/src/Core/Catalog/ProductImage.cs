using NexCart.Domain.Common.Entities;

namespace NexCart.Domain.Catalog;

public sealed class ProductImage : Entity<ProductImageId>
{
    public ProductId ProductId { get; private set; }
    public string Url { get; private set; }
    public string? AltText { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsPrimary { get; private set; }

    private ProductImage() : base()
    {
        ProductId = null!;
        Url = string.Empty;
    }

    private ProductImage(
        ProductImageId id,
        ProductId productId,
        string url,
        string? altText,
        int displayOrder,
        bool isPrimary)
        : base(id)
    {
        ProductId = productId;
        Url = url;
        AltText = altText;
        DisplayOrder = displayOrder;
        IsPrimary = isPrimary;
    }

    public static ProductImage Create(
        ProductId productId,
        string url,
        string? altText = null,
        int displayOrder = 0,
        bool isPrimary = false)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("La URL de la imagen es requerida", nameof(url));

        if (displayOrder < 0)
            throw new ArgumentException("El orden de visualización no puede ser negativo", nameof(displayOrder));

        return new ProductImage(
            ProductImageId.CreateUnique(),
            productId,
            url.Trim(),
            altText?.Trim(),
            displayOrder,
            isPrimary);
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void UnsetAsPrimary()
    {
        IsPrimary = false;
    }

    public void SetDisplayOrder(int order)
    {
        if (order < 0)
            throw new ArgumentException("El orden de visualización no puede ser negativo", nameof(order));

        DisplayOrder = order;
    }

    public void UpdateAltText(string altText)
    {
        AltText = altText?.Trim();
    }
}