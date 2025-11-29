using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Orders.ValueObjects;

public sealed class OrderNumber : ValueObject
{
    public string Value { get; }

    private OrderNumber(string value)
    {
        Value = value;
    }

    public static OrderNumber Create(string prefix = "ORD")
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        var orderNumber = $"{prefix}-{timestamp}-{random}";

        return new OrderNumber(orderNumber);
    }

    public static OrderNumber Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El número de orden no puede estar vacío", nameof(value));

        return new OrderNumber(value.Trim().ToUpperInvariant());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(OrderNumber orderNumber) => orderNumber.Value;
}