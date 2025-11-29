using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Orders;

public sealed class OrderItemId : ValueObject
{
    public Guid Value { get; }

    private OrderItemId(Guid value)
    {
        Value = value;
    }

    public static OrderItemId CreateUnique() => new(Guid.NewGuid());

    public static OrderItemId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID del item de la orden no puede estar vacío", nameof(value));

        return new OrderItemId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(OrderItemId itemId) => itemId.Value;
}