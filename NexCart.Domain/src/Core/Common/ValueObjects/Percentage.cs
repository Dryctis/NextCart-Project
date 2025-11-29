namespace NexCart.Domain.Common.ValueObjects;

public sealed class Percentage : ValueObject
{

    public decimal Value { get; }

    private Percentage(decimal value)
    {
        Value = value;
    }

  
    public static Percentage FromDecimal(decimal value)
    {
        if (value < 0 || value > 1)
            throw new ArgumentException("Percentage must be between 0 and 1", nameof(value));

        return new Percentage(value);
    }

    public static Percentage FromPercentage(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("Percentage must be between 0 and 100", nameof(percentage));

        return new Percentage(percentage / 100m);
    }

 
    public decimal ToPercentage() => Value * 100m;

   
    public Money ApplyTo(Money amount)
    {
        return amount.Multiply(Value);
    }

    public static Percentage Zero() => new(0);
    public static Percentage OneHundred() => new(1);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => $"{ToPercentage():N2}%";
}