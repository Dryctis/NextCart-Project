using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Customers;

public sealed class CustomerAddressId : ValueObject
{
    public Guid Value { get; }

    private CustomerAddressId(Guid value)
    {
        Value = value;
    }

    public static CustomerAddressId CreateUnique() => new(Guid.NewGuid());

    public static CustomerAddressId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID de la dirección no puede estar vacío", nameof(value));

        return new CustomerAddressId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(CustomerAddressId addressId) => addressId.Value;
}