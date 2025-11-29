using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Orders;

public sealed class ShippingInfo : ValueObject
{
    public string FullName { get; }
    public string Email { get; }
    public string Phone { get; }
    public Address Address { get; }

    private ShippingInfo(string fullName, string email, string phone, Address address)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        Address = address;
    }

    public static ShippingInfo Create(
        string fullName,
        string email,
        string phone,
        Address address)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("El nombre completo es requerido", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email es requerido", nameof(email));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("El teléfono es requerido", nameof(phone));

        return new ShippingInfo(
            fullName.Trim(),
            email.Trim(),
            phone.Trim(),
            address);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FullName;
        yield return Email;
        yield return Phone;
        yield return Address;
    }
}