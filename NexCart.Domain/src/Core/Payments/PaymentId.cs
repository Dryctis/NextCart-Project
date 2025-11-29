using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Payments;

public sealed class PaymentId : ValueObject
{
    public Guid Value { get; }

    private PaymentId(Guid value)
    {
        Value = value;
    }

    public static PaymentId CreateUnique() => new(Guid.NewGuid());

    public static PaymentId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID del pago no puede estar vacío", nameof(value));

        return new PaymentId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(PaymentId paymentId) => paymentId.Value;
}