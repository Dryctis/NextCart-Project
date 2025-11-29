using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Shopping;

public sealed class ShoppingCartId : ValueObject
{
    public Guid Value { get; }

    private ShoppingCartId(Guid value)
    {
        Value = value;
    }

    public static ShoppingCartId CreateUnique() => new(Guid.NewGuid());

    public static ShoppingCartId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID del carrito no puede estar vacío", nameof(value));

        return new ShoppingCartId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(ShoppingCartId cartId) => cartId.Value;
}