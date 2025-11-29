using NexCart.Domain.Common.Entities;
using NexCart.Domain.Common.ValueObjects;
using NexCart.Domain.Customers.Enums;
using NexCart.Domain.Customers.Events;
using NexCart.Domain.Customers.ValueObjects;
using NexCart.Domain.Tenants;

namespace NexCart.Domain.Customers;

public sealed class Customer : AggregateRoot<CustomerId>, IAuditableEntity, ISoftDeletable
{
    public TenantId TenantId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber? Phone { get; private set; }
    public CustomerStatus Status { get; private set; }
    public LoyaltyPoints LoyaltyPoints { get; private set; }
    public DateTime? LastOrderDate { get; private set; }
    public int TotalOrders { get; private set; }
    public Money? TotalSpent { get; private set; }
    public bool EmailVerified { get; private set; }
    public DateTime? EmailVerifiedAt { get; private set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    private readonly List<CustomerAddress> _addresses = new();
    public IReadOnlyCollection<CustomerAddress> Addresses => _addresses.AsReadOnly();

    private Customer() : base()
    {
        TenantId = null!;
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = null!;
        LoyaltyPoints = LoyaltyPoints.Zero();
        CreatedBy = string.Empty;
    }

    private Customer(
        CustomerId id,
        TenantId tenantId,
        string firstName,
        string lastName,
        Email email,
        PhoneNumber? phone)
        : base(id)
    {
        TenantId = tenantId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Status = CustomerStatus.Active;
        LoyaltyPoints = LoyaltyPoints.Zero();
        TotalOrders = 0;
        EmailVerified = false;
        CreatedBy = string.Empty;

        AddDomainEvent(new CustomerRegisteredEvent(id, email.Value, GetFullName()));
    }

    public static Customer Create(
        TenantId tenantId,
        string firstName,
        string lastName,
        string email,
        string? phone = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("El nombre es requerido", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("El apellido es requerido", nameof(lastName));

        if (firstName.Length > 50)
            throw new ArgumentException("El nombre es muy largo", nameof(firstName));

        if (lastName.Length > 50)
            throw new ArgumentException("El apellido es muy largo", nameof(lastName));

        var emailValueObject = Email.Create(email);
        var phoneValueObject = phone != null ? PhoneNumber.Create(phone) : null;

        return new Customer(
            CustomerId.CreateUnique(),
            tenantId,
            firstName.Trim(),
            lastName.Trim(),
            emailValueObject,
            phoneValueObject);
    }

    public void UpdateProfile(string firstName, string lastName, string? phone)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("El nombre es requerido", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("El apellido es requerido", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Phone = phone != null ? PhoneNumber.Create(phone) : null;
    }

    public void AddAddress(
        string fullName,
        string phone,
        Address address,
        bool setAsDefault = false,
        string? label = null)
    {
        var customerAddress = CustomerAddress.Create(
            Id,
            fullName,
            phone,
            address,
            setAsDefault,
            label);

        if (setAsDefault)
        {
            foreach (var addr in _addresses)
            {
                addr.UnsetAsDefault();
            }
        }

        if (_addresses.Count == 0)
        {
            customerAddress.SetAsDefault();
        }

        _addresses.Add(customerAddress);

        AddDomainEvent(new CustomerAddressAddedEvent(Id, customerAddress.Id));
    }

    public void RemoveAddress(CustomerAddressId addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id.Equals(addressId));

        if (address == null)
            throw new InvalidOperationException("Dirección no encontrada");

        _addresses.Remove(address);

        if (address.IsDefault && _addresses.Count > 0)
        {
            _addresses.First().SetAsDefault();
        }
    }

    public void SetDefaultAddress(CustomerAddressId addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id.Equals(addressId));

        if (address == null)
            throw new InvalidOperationException("Dirección no encontrada");

        foreach (var addr in _addresses)
        {
            addr.UnsetAsDefault();
        }

        address.SetAsDefault();
    }

    public void VerifyEmail()
    {
        if (EmailVerified)
            return;

        EmailVerified = true;
        EmailVerifiedAt = DateTime.UtcNow;
    }

    public void AddLoyaltyPoints(int points)
    {
        if (points <= 0)
            throw new ArgumentException("Los puntos deben ser mayores a cero", nameof(points));

        LoyaltyPoints = LoyaltyPoints.Add(points);
    }

    public void RedeemLoyaltyPoints(int points)
    {
        if (points <= 0)
            throw new ArgumentException("Los puntos deben ser mayores a cero", nameof(points));

        LoyaltyPoints = LoyaltyPoints.Subtract(points);
    }

    public void RecordOrder(Money orderTotal)
    {
        TotalOrders++;
        LastOrderDate = DateTime.UtcNow;

        if (TotalSpent == null)
        {
            TotalSpent = orderTotal;
        }
        else
        {
            TotalSpent = TotalSpent.Add(orderTotal);
        }
    }

    public void Block()
    {
        if (Status == CustomerStatus.Blocked)
            throw new InvalidOperationException("El cliente ya está bloqueado");

        Status = CustomerStatus.Blocked;
    }

    public void Unblock()
    {
        if (Status != CustomerStatus.Blocked)
            throw new InvalidOperationException("El cliente no está bloqueado");

        Status = CustomerStatus.Active;
    }

    public void Deactivate()
    {
        Status = CustomerStatus.Inactive;
    }

    public void Activate()
    {
        Status = CustomerStatus.Active;
    }

    public string GetFullName() => $"{FirstName} {LastName}";

    public CustomerAddress? GetDefaultAddress() => _addresses.FirstOrDefault(a => a.IsDefault);

    public bool HasAddresses() => _addresses.Count > 0;

    public bool IsActive() => Status == CustomerStatus.Active;
}