using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Customers;

public sealed class CustomerId : ValueObject
{
    public Guid Value { get; }

    private CustomerId(Guid value)
    {
        Value = value;
    }

    public static CustomerId CreateUnique() => new(Guid.NewGuid());

    public static CustomerId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID del cliente no puede estar vacío", nameof(value));

        return new CustomerId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(CustomerId customerId) => customerId.Value;
}