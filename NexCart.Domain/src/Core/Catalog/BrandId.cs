using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog;

public sealed class BrandId : ValueObject
{
    public Guid Value { get; }

    private BrandId(Guid value)
    {
        Value = value;
    }

    public static BrandId CreateUnique() => new(Guid.NewGuid());

    public static BrandId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID de la marca no puede estar vacío", nameof(value));

        return new BrandId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(BrandId brandId) => brandId.Value;
}