using NexCart.Domain.Catalog;
using NexCart.Domain.Common.Entities;
using NexCart.Domain.Common.ValueObjects;
using NexCart.Domain.Orders.Enums;
using NexCart.Domain.Orders.Events;
using NexCart.Domain.Orders.ValueObjects;
using NexCart.Domain.Tenants;

namespace NexCart.Domain.Orders;

public sealed class Order : AggregateRoot<OrderId>, IAuditableEntity
{
    public TenantId TenantId { get; private set; }
    public OrderNumber OrderNumber { get; private set; }
    public string CustomerId { get; private set; }
    public string CustomerEmail { get; private set; }
    public OrderStatus Status { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public FulfillmentType FulfillmentType { get; private set; }
    public Money Subtotal { get; private set; }
    public Money? Discount { get; private set; }
    public Money ShippingCost { get; private set; }
    public Money Tax { get; private set; }
    public Money Total { get; private set; }
    public ShippingInfo? ShippingInfo { get; private set; }
    public string? PaymentMethod { get; private set; }
    public string? PaymentTransactionId { get; private set; }
    public TrackingNumber? TrackingNumber { get; private set; }
    public string? Notes { get; private set; }
    public string? CancellationReason { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? ShippedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() : base()
    {
        TenantId = null!;
        OrderNumber = null!;
        CustomerId = string.Empty;
        CustomerEmail = string.Empty;
        PaymentStatus = Enums.PaymentStatus.Pending;
        Subtotal = Money.Zero();
        ShippingCost = Money.Zero();
        Tax = Money.Zero();
        Total = Money.Zero();
        CreatedBy = string.Empty;
    }

    private Order(
        OrderId id,
        TenantId tenantId,
        OrderNumber orderNumber,
        string customerId,
        string customerEmail,
        FulfillmentType fulfillmentType,
        ShippingInfo? shippingInfo,
        List<OrderItem> items)
        : base(id)
    {
        TenantId = tenantId;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        CustomerEmail = customerEmail;
        Status = OrderStatus.Pending;
        PaymentStatus = Enums.PaymentStatus.Pending;
        FulfillmentType = fulfillmentType;
        ShippingInfo = shippingInfo;
        _items = items;
        CreatedBy = string.Empty;

        CalculateTotals();

        AddDomainEvent(new OrderPlacedEvent(id, orderNumber.Value, customerId, Total));
    }

    public static Order Create(
        TenantId tenantId,
        string customerId,
        string customerEmail,
        FulfillmentType fulfillmentType,
        ShippingInfo? shippingInfo,
        List<OrderItem> items)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentException("El ID del cliente es requerido", nameof(customerId));

        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new ArgumentException("El email del cliente es requerido", nameof(customerEmail));

        if (items == null || items.Count == 0)
            throw new ArgumentException("La orden debe tener al menos un item", nameof(items));

        if (fulfillmentType != FulfillmentType.DigitalDownload &&
            fulfillmentType != FulfillmentType.StorePickup &&
            shippingInfo == null)
        {
            throw new ArgumentException("La información de envío es requerida para este tipo de entrega");
        }

        var orderNumber = OrderNumber.Create();

        return new Order(
            OrderId.CreateUnique(),
            tenantId,
            orderNumber,
            customerId.Trim(),
            customerEmail.Trim(),
            fulfillmentType,
            shippingInfo,
            items);
    }

    public void ConfirmPayment(string paymentMethod, string transactionId)
    {
        if (PaymentStatus != Enums.PaymentStatus.Pending)
            throw new InvalidOperationException("El pago ya ha sido procesado");

        if (string.IsNullOrWhiteSpace(paymentMethod))
            throw new ArgumentException("El método de pago es requerido", nameof(paymentMethod));

        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException("El ID de transacción es requerido", nameof(transactionId));

        PaymentMethod = paymentMethod.Trim();
        PaymentTransactionId = transactionId.Trim();
        PaymentStatus = Enums.PaymentStatus.Paid;
        Status = OrderStatus.Confirmed;
        PaidAt = DateTime.UtcNow;

        AddDomainEvent(new OrderPaidEvent(Id, paymentMethod));
    }

    public void MarkAsProcessing()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Solo las órdenes confirmadas pueden procesarse");

        Status = OrderStatus.Processing;
    }

    public void MarkAsShipped(string? trackingNumber = null)
    {
        if (Status != OrderStatus.Processing && Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Solo las órdenes confirmadas o en procesamiento pueden enviarse");

        if (FulfillmentType == FulfillmentType.DigitalDownload)
            throw new InvalidOperationException("Los productos digitales no se envían");

        Status = OrderStatus.Shipped;
        ShippedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(trackingNumber))
        {
            TrackingNumber = ValueObjects.TrackingNumber.Create(trackingNumber);
        }

        AddDomainEvent(new OrderShippedEvent(Id, trackingNumber));
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Solo las órdenes enviadas pueden marcarse como entregadas");

        Status = OrderStatus.Delivered;
        DeliveredAt = DateTime.UtcNow;

        AddDomainEvent(new OrderDeliveredEvent(Id));
    }

    public void Cancel(string reason)
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("La orden ya está cancelada");

        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("No se puede cancelar una orden ya entregada");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("La razón de cancelación es requerida", nameof(reason));

        Status = OrderStatus.Cancelled;
        CancellationReason = reason.Trim();
        CancelledAt = DateTime.UtcNow;

        if (PaymentStatus == Enums.PaymentStatus.Paid)
        {
            PaymentStatus = Enums.PaymentStatus.Cancelled;
        }

        AddDomainEvent(new OrderCancelledEvent(Id, reason));
    }

    public void ApplyDiscount(Money discount)
    {
        if (discount < Money.Zero(Total.Currency))
            throw new ArgumentException("El descuento no puede ser negativo");

        if (discount > Subtotal)
            throw new ArgumentException("El descuento no puede ser mayor al subtotal");

        Discount = discount;
        CalculateTotals();
    }

    public void SetShippingCost(Money shippingCost)
    {
        if (shippingCost < Money.Zero(Total.Currency))
            throw new ArgumentException("El costo de envío no puede ser negativo");

        ShippingCost = shippingCost;
        CalculateTotals();
    }

    public void SetTax(Money tax)
    {
        if (tax < Money.Zero(Total.Currency))
            throw new ArgumentException("El impuesto no puede ser negativo");

        Tax = tax;
        CalculateTotals();
    }

    public void AddNotes(string notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
            throw new ArgumentException("Las notas no pueden estar vacías", nameof(notes));

        Notes = notes.Trim();
    }

    public bool CanBeCancelled()
    {
        return Status != OrderStatus.Cancelled &&
               Status != OrderStatus.Delivered &&
               Status != OrderStatus.Refunded;
    }

    public bool RequiresShipping()
    {
        return FulfillmentType != FulfillmentType.DigitalDownload &&
               FulfillmentType != FulfillmentType.StorePickup;
    }

    public int TotalItems => _items.Sum(i => i.Quantity);

    private void CalculateTotals()
    {
        if (_items.Count == 0)
        {
            Subtotal = Money.Zero();
            ShippingCost = Money.Zero();
            Tax = Money.Zero();
            Total = Money.Zero();
            return;
        }

        var firstItem = _items.First();
        var currency = firstItem.UnitPrice.Currency;

        var subtotal = Money.Zero(currency);
        foreach (var item in _items)
        {
            subtotal = subtotal.Add(item.Total);
        }

        Subtotal = subtotal;

        if (ShippingCost == null)
            ShippingCost = Money.Zero(currency);

        if (Tax == null)
            Tax = Money.Zero(currency);

        var total = Subtotal.Add(ShippingCost).Add(Tax);

        if (Discount != null)
            total = total.Subtract(Discount);

        Total = total;
    }
}