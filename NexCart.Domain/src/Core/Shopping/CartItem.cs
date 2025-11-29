using NexCart.Domain.Catalog;
using NexCart.Domain.Common.Entities;
using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Shopping;

public sealed class CartItem : Entity<CartItemId>
{
    public ShoppingCartId ShoppingCartId { get; private set; }
    public ProductId ProductId { get; private set; }
    public ProductVariantId? ProductVariantId { get; private set; }
    public string ProductName { get; private set; }
    public string? VariantName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public Money Subtotal { get; private set; }
    public string? ImageUrl { get; private set; }
    public DateTime AddedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private CartItem() : base()
    {
        ShoppingCartId = null!;
        ProductId = null!;
        ProductName = string.Empty;
        UnitPrice = null!;
        Subtotal = null!;
    }

    private CartItem(
        CartItemId id,
        ShoppingCartId shoppingCartId,
        ProductId productId,
        ProductVariantId? productVariantId,
        string productName,
        string? variantName,
        Money unitPrice,
        int quantity,
        string? imageUrl)
        : base(id)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        ProductVariantId = productVariantId;
        ProductName = productName;
        VariantName = variantName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Subtotal = unitPrice * quantity;
        ImageUrl = imageUrl;
        AddedAt = DateTime.UtcNow;
    }

    public static CartItem Create(
        ShoppingCartId shoppingCartId,
        ProductId productId,
        ProductVariantId? productVariantId,
        string productName,
        string? variantName,
        Money unitPrice,
        int quantity,
        string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("El nombre del producto es requerido", nameof(productName));

        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(quantity));

        if (quantity > 999)
            throw new ArgumentException("La cantidad máxima por item es 999", nameof(quantity));

        return new CartItem(
            CartItemId.CreateUnique(),
            shoppingCartId,
            productId,
            productVariantId,
            productName.Trim(),
            variantName?.Trim(),
            unitPrice,
            quantity,
            imageUrl);
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(newQuantity));

        if (newQuantity > 999)
            throw new ArgumentException("La cantidad máxima por item es 999", nameof(newQuantity));

        Quantity = newQuantity;
        Subtotal = UnitPrice * newQuantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncreaseQuantity(int amount = 1)
    {
        if (amount <= 0)
            throw new ArgumentException("El incremento debe ser mayor a cero", nameof(amount));

        var newQuantity = Quantity + amount;

        if (newQuantity > 999)
            throw new InvalidOperationException("La cantidad máxima por item es 999");

        UpdateQuantity(newQuantity);
    }

    public void DecreaseQuantity(int amount = 1)
    {
        if (amount <= 0)
            throw new ArgumentException("El decremento debe ser mayor a cero", nameof(amount));

        var newQuantity = Quantity - amount;

        if (newQuantity < 1)
            throw new InvalidOperationException("La cantidad mínima es 1. Use Remove para eliminar el item");

        UpdateQuantity(newQuantity);
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice <= Money.Zero(UnitPrice.Currency))
            throw new ArgumentException("El precio debe ser mayor a cero");

        UnitPrice = newPrice;
        Subtotal = newPrice * Quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}