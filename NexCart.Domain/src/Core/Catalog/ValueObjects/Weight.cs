using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog.ValueObjects;

public sealed class Weight : ValueObject
{
    public decimal Value { get; }
    public string Unit { get; }

    private Weight(decimal value, string unit)
    {
        Value = value;
        Unit = unit;
    }

    public static Weight Create(decimal value, string unit = "kg")
    {
        if (value < 0)
            throw new ArgumentException("El peso no puede ser negativo", nameof(value));

        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("La unidad de medida es requerida", nameof(unit));

        return new Weight(value, unit.ToLowerInvariant());
    }

    public Weight ConvertTo(string targetUnit)
    {
        if (Unit == targetUnit)
            return this;

        var valueInKg = Unit switch
        {
            "g" => Value / 1000m,
            "kg" => Value,
            "lb" => Value * 0.453592m,
            "oz" => Value * 0.0283495m,
            _ => throw new InvalidOperationException($"Unidad no soportada: {Unit}")
        };

        var convertedValue = targetUnit switch
        {
            "g" => valueInKg * 1000m,
            "kg" => valueInKg,
            "lb" => valueInKg / 0.453592m,
            "oz" => valueInKg / 0.0283495m,
            _ => throw new InvalidOperationException($"Unidad no soportada: {targetUnit}")
        };

        return new Weight(convertedValue, targetUnit);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Unit;
    }

    public override string ToString() => $"{Value} {Unit}";
}