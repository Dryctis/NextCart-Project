using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Orders.ValueObjects;

public sealed class TrackingNumber : ValueObject
{
    public string Value { get; }

    private TrackingNumber(string value)
    {
        Value = value;
    }

    public static TrackingNumber Create(string trackingNumber)
    {
        if (string.IsNullOrWhiteSpace(trackingNumber))
            throw new ArgumentException("El número de rastreo no puede estar vacío", nameof(trackingNumber));

        return new TrackingNumber(trackingNumber.Trim().ToUpperInvariant());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(TrackingNumber trackingNumber) => trackingNumber.Value;
}