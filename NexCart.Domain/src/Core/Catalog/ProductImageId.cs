using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog;

public sealed class ProductImageId : ValueObject
{
    public Guid Value { get; }

    private ProductImageId(Guid value)
    {
        Value = value;
    }

    public static ProductImageId CreateUnique() => new(Guid.NewGuid());

    public static ProductImageId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID de la imagen no puede estar vacío", nameof(value));

        return new ProductImageId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(ProductImageId imageId) => imageId.Value;
}