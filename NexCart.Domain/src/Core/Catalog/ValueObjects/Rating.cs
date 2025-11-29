using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog.ValueObjects;

public sealed class Rating : ValueObject
{
    public decimal Value { get; }
    public int ReviewCount { get; }

    private Rating(decimal value, int reviewCount)
    {
        Value = value;
        ReviewCount = reviewCount;
    }

    public static Rating Create(decimal value, int reviewCount)
    {
        if (value < 0 || value > 5)
            throw new ArgumentException("La calificación debe estar entre 0 y 5", nameof(value));

        if (reviewCount < 0)
            throw new ArgumentException("El número de reseñas no puede ser negativo", nameof(reviewCount));

        return new Rating(value, reviewCount);
    }

    public static Rating Empty() => new(0, 0);

    public Rating AddReview(int stars)
    {
        if (stars < 1 || stars > 5)
            throw new ArgumentException("La calificación debe estar entre 1 y 5", nameof(stars));

        var totalPoints = (Value * ReviewCount) + stars;
        var newReviewCount = ReviewCount + 1;
        var newAverage = totalPoints / newReviewCount;

        return new Rating(newAverage, newReviewCount);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return ReviewCount;
    }

    public override string ToString() => $"{Value:F1} ({ReviewCount} reseñas)";
}