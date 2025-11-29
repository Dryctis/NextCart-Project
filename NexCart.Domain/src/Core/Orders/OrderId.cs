using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Orders;

public sealed class OrderId : ValueObject
{
    public Guid Value { get; }

    private OrderId(Guid value)
    {
        Value = value;
    }

    public static OrderId CreateUnique() => new(Guid.NewGuid());

    public static OrderId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID de la orden no puede estar vacío", nameof(value));

        return new OrderId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(OrderId orderId) => orderId.Value;
}