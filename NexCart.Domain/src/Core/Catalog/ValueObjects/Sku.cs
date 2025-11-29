using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog.ValueObjects;

public sealed class Sku : ValueObject
{
    public string Value { get; }

    private Sku(string value)
    {
        Value = value;
    }

    public static Sku Create(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("El SKU no puede estar vacío", nameof(sku));

        sku = sku.Trim().ToUpperInvariant();

        if (sku.Length < 3 || sku.Length > 50)
            throw new ArgumentException("El SKU debe tener entre 3 y 50 caracteres", nameof(sku));

        if (!IsValidSku(sku))
            throw new ArgumentException("El SKU solo puede contener letras, números y guiones", nameof(sku));

        return new Sku(sku);
    }

    private static bool IsValidSku(string sku)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(sku, @"^[A-Z0-9\-]+$");
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Sku sku) => sku.Value;
}