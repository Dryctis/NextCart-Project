using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Shopping;

public sealed class WishlistId : ValueObject
{
    public Guid Value { get; }

    private WishlistId(Guid value)
    {
        Value = value;
    }

    public static WishlistId CreateUnique() => new(Guid.NewGuid());

    public static WishlistId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID de la lista de deseos no puede estar vacío", nameof(value));

        return new WishlistId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(WishlistId wishlistId) => wishlistId.Value;
}