using NexCart.Domain.Catalog;
using NexCart.Domain.Common.Entities;
using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Orders;

public sealed class OrderItem : Entity<OrderItemId>
{
    public OrderId OrderId { get; private set; }
    public ProductId ProductId { get; private set; }
    public ProductVariantId? ProductVariantId { get; private set; }
    public string ProductName { get; private set; }
    public string? VariantName { get; private set; }
    public string Sku { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public Money Subtotal { get; private set; }
    public Money? Discount { get; private set; }
    public Money Total { get; private set; }
    public string? ImageUrl { get; private set; }

    private OrderItem() : base()
    {
        OrderId = null!;
        ProductId = null!;
        ProductName = string.Empty;
        Sku = string.Empty;
        UnitPrice = null!;
        Subtotal = null!;
        Total = null!;
    }

    private OrderItem(
        OrderItemId id,
        OrderId orderId,
        ProductId productId,
        ProductVariantId? productVariantId,
        string productName,
        string? variantName,
        string sku,
        Money unitPrice,
        int quantity,
        Money? discount,
        string? imageUrl)
        : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductVariantId = productVariantId;
        ProductName = productName;
        VariantName = variantName;
        Sku = sku;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Discount = discount;
        Subtotal = unitPrice * quantity;
        Total = discount != null ? Subtotal - discount : Subtotal;
        ImageUrl = imageUrl;
    }

    public static OrderItem Create(
        OrderId orderId,
        ProductId productId,
        ProductVariantId? productVariantId,
        string productName,
        string? variantName,
        string sku,
        Money unitPrice,
        int quantity,
        Money? discount = null,
        string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("El nombre del producto es requerido", nameof(productName));

        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("El SKU es requerido", nameof(sku));

        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(quantity));

        return new OrderItem(
            OrderItemId.CreateUnique(),
            orderId,
            productId,
            productVariantId,
            productName.Trim(),
            variantName?.Trim(),
            sku.Trim(),
            unitPrice,
            quantity,
            discount,
            imageUrl);
    }
}