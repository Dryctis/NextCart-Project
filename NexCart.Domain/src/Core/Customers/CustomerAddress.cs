using NexCart.Domain.Common.Entities;
using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Customers;

public sealed class CustomerAddress : Entity<CustomerAddressId>
{
    public CustomerId CustomerId { get; private set; }
    public string FullName { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Address Address { get; private set; }
    public bool IsDefault { get; private set; }
    public string? Label { get; private set; }

    private CustomerAddress() : base()
    {
        CustomerId = null!;
        FullName = string.Empty;
        Phone = null!;
        Address = null!;
    }

    private CustomerAddress(
        CustomerAddressId id,
        CustomerId customerId,
        string fullName,
        PhoneNumber phone,
        Address address,
        bool isDefault,
        string? label)
        : base(id)
    {
        CustomerId = customerId;
        FullName = fullName;
        Phone = phone;
        Address = address;
        IsDefault = isDefault;
        Label = label;
    }

    public static CustomerAddress Create(
        CustomerId customerId,
        string fullName,
        string phone,
        Address address,
        bool isDefault = false,
        string? label = null)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("El nombre completo es requerido", nameof(fullName));

        var phoneNumber = PhoneNumber.Create(phone);

        return new CustomerAddress(
            CustomerAddressId.CreateUnique(),
            customerId,
            fullName.Trim(),
            phoneNumber,
            address,
            isDefault,
            label?.Trim());
    }

    public void SetAsDefault()
    {
        IsDefault = true;
    }

    public void UnsetAsDefault()
    {
        IsDefault = false;
    }

    public void UpdateDetails(string fullName, string phone, Address address, string? label)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("El nombre completo es requerido", nameof(fullName));

        FullName = fullName.Trim();
        Phone = PhoneNumber.Create(phone);
        Address = address;
        Label = label?.Trim();
    }
}