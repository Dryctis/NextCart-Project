using System.Text.RegularExpressions;

namespace NexCart.Domain.Common.ValueObjects;

public sealed class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

  
    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        email = email.Trim().ToLowerInvariant();

        if (email.Length > 255)
            throw new ArgumentException("Email is too long", nameof(email));

        if (!EmailRegex.IsMatch(email))
            throw new ArgumentException("Email format is invalid", nameof(email));

        return new Email(email);
    }

    public static Email? TryCreate(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        try
        {
            return Create(email);
        }
        catch
        {
            return null;
        }
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}