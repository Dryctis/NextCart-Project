using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Tenants;

public sealed class TenantId : ValueObject
{
    public Guid Value { get; }

    private TenantId(Guid value)
    {
        Value = value;
    }

    public static TenantId CreateUnique() => new(Guid.NewGuid());

    public static TenantId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID del tenant no puede estar vacío", nameof(value));

        return new TenantId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(TenantId tenantId) => tenantId.Value;
}