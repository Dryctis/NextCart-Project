using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Shopping;

public sealed class WishlistItemId : ValueObject
{
    public Guid Value { get; }

    private WishlistItemId(Guid value)
    {
        Value = value;
    }

    public static WishlistItemId CreateUnique() => new(Guid.NewGuid());

    public static WishlistItemId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID del item de la lista no puede estar vacío", nameof(value));

        return new WishlistItemId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(WishlistItemId itemId) => itemId.Value;
}