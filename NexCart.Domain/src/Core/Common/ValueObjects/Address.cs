namespace NexCart.Domain.Common.ValueObjects;

public sealed class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }
    public string Country { get; }

    private Address(
        string street,
        string city,
        string state,
        string zipCode,
        string country)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
    }

    public static Address Create(
        string street,
        string city,
        string state,
        string zipCode,
        string country)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street is required", nameof(street));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required", nameof(city));

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required", nameof(country));

        if (street.Length > 200)
            throw new ArgumentException("Street is too long", nameof(street));

        if (city.Length > 100)
            throw new ArgumentException("City is too long", nameof(city));

        return new Address(
            street.Trim(),
            city.Trim(),
            state?.Trim() ?? string.Empty,
            zipCode?.Trim() ?? string.Empty,
            country.Trim());
    }

   
    public string ToSingleLine()
    {
        var parts = new List<string> { Street, City };

        if (!string.IsNullOrWhiteSpace(State))
            parts.Add(State);

        if (!string.IsNullOrWhiteSpace(ZipCode))
            parts.Add(ZipCode);

        parts.Add(Country);

        return string.Join(", ", parts);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
        yield return Country;
    }

    public override string ToString() => ToSingleLine();
}