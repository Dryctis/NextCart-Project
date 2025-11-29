using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog;

public sealed class ProductId : ValueObject
{
    public Guid Value { get; }

    private ProductId(Guid value)
    {
        Value = value;
    }

    public static ProductId CreateUnique() => new(Guid.NewGuid());

    public static ProductId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID del producto no puede estar vacío", nameof(value));

        return new ProductId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(ProductId productId) => productId.Value;
}