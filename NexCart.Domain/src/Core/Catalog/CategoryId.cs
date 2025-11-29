using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog;

public sealed class CategoryId : ValueObject
{
    public Guid Value { get; }

    private CategoryId(Guid value)
    {
        Value = value;
    }

    public static CategoryId CreateUnique() => new(Guid.NewGuid());

    public static CategoryId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID de la categoría no puede estar vacío", nameof(value));

        return new CategoryId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(CategoryId categoryId) => categoryId.Value;
}