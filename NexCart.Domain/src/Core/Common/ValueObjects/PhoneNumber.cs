namespace NexCart.Domain.Common.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

 
    public static PhoneNumber Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

        var cleaned = new string(phoneNumber.Where(char.IsDigit).ToArray());

        if (cleaned.Length < 7)
            throw new ArgumentException("Phone number is too short", nameof(phoneNumber));

        if (cleaned.Length > 15)
            throw new ArgumentException("Phone number is too long", nameof(phoneNumber));

        return new PhoneNumber(cleaned);
    }

    public static PhoneNumber? TryCreate(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return null;

        try
        {
            return Create(phoneNumber);
        }
        catch
        {
            return null;
        }
    }

    
    public string Format()
    {
        if (Value.Length == 10)
            return $"({Value.Substring(0, 3)}) {Value.Substring(3, 3)}-{Value.Substring(6)}";

        if (Value.Length == 11 && Value.StartsWith("1"))
            return $"+1 ({Value.Substring(1, 3)}) {Value.Substring(4, 3)}-{Value.Substring(7)}";

        return Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Format();

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
}