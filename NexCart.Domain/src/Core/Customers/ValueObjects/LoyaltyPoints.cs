using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Customers.ValueObjects;

public sealed class LoyaltyPoints : ValueObject
{
    public int Points { get; }

    private LoyaltyPoints(int points)
    {
        Points = points;
    }

    public static LoyaltyPoints Create(int points = 0)
    {
        if (points < 0)
            throw new ArgumentException("Los puntos no pueden ser negativos", nameof(points));

        return new LoyaltyPoints(points);
    }

    public static LoyaltyPoints Zero() => new(0);

    public LoyaltyPoints Add(int points)
    {
        if (points <= 0)
            throw new ArgumentException("Los puntos a agregar deben ser mayores a cero", nameof(points));

        return new LoyaltyPoints(Points + points);
    }

    public LoyaltyPoints Subtract(int points)
    {
        if (points <= 0)
            throw new ArgumentException("Los puntos a restar deben ser mayores a cero", nameof(points));

        if (Points < points)
            throw new InvalidOperationException("No hay suficientes puntos disponibles");

        return new LoyaltyPoints(Points - points);
    }

    public bool HasEnough(int requiredPoints)
    {
        return Points >= requiredPoints;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Points;
    }

    public override string ToString() => $"{Points} puntos";
}