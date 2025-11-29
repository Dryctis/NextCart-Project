using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog;

public sealed class ProductVariantId : ValueObject
{
    public Guid Value { get; }

    private ProductVariantId(Guid value)
    {
        Value = value;
    }

    public static ProductVariantId CreateUnique() => new(Guid.NewGuid());

    public static ProductVariantId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID de la variante no puede estar vacío", nameof(value));

        return new ProductVariantId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(ProductVariantId variantId) => variantId.Value;
}