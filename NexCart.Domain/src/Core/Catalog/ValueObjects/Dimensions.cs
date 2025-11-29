using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog.ValueObjects;

public sealed class Dimensions : ValueObject
{
    public decimal Length { get; }
    public decimal Width { get; }
    public decimal Height { get; }
    public string Unit { get; }

    private Dimensions(decimal length, decimal width, decimal height, string unit)
    {
        Length = length;
        Width = width;
        Height = height;
        Unit = unit;
    }

    public static Dimensions Create(decimal length, decimal width, decimal height, string unit = "cm")
    {
        if (length <= 0)
            throw new ArgumentException("El largo debe ser mayor a cero", nameof(length));

        if (width <= 0)
            throw new ArgumentException("El ancho debe ser mayor a cero", nameof(width));

        if (height <= 0)
            throw new ArgumentException("La altura debe ser mayor a cero", nameof(height));

        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("La unidad de medida es requerida", nameof(unit));

        return new Dimensions(length, width, height, unit.ToLowerInvariant());
    }

    public decimal CalculateVolume()
    {
        return Length * Width * Height;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Length;
        yield return Width;
        yield return Height;
        yield return Unit;
    }

    public override string ToString() => $"{Length}x{Width}x{Height} {Unit}";
}