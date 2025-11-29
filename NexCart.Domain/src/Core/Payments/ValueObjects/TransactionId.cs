using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Payments.ValueObjects;

public sealed class TransactionId : ValueObject
{
    public string Value { get; }

    private TransactionId(string value)
    {
        Value = value;
    }

    public static TransactionId Create(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException("El ID de transacción no puede estar vacío", nameof(transactionId));

        return new TransactionId(transactionId.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(TransactionId transactionId) => transactionId.Value;
}