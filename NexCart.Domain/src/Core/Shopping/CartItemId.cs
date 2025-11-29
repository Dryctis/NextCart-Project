using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Shopping;

public sealed class CartItemId : ValueObject
{
    public Guid Value { get; }

    private CartItemId(Guid value)
    {
        Value = value;
    }

    public static CartItemId CreateUnique() => new(Guid.NewGuid());

    public static CartItemId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID del item del carrito no puede estar vacío", nameof(value));

        return new CartItemId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(CartItemId itemId) => itemId.Value;
}