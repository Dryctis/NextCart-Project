using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Catalog;
using NexCart.Domain.Orders;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => OrderItemId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(i => i.OrderId)
            .HasConversion(
                id => id.Value,
                value => OrderId.Of(value))
            .IsRequired();

        builder.Property(i => i.ProductId)
            .HasConversion(
                id => id.Value,
                value => ProductId.Of(value))
            .IsRequired();

        builder.Property(i => i.ProductVariantId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? ProductVariantId.Of(value.Value) : null);

        builder.Property(i => i.ProductName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.VariantName)
            .HasMaxLength(200);

        builder.Property(i => i.Sku)
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(i => i.UnitPrice, unitPrice =>
        {
            unitPrice.Property(m => m.Amount)
                .HasColumnName("UnitPrice")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            unitPrice.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.OwnsOne(i => i.Subtotal, subtotal =>
        {
            subtotal.Property(m => m.Amount)
                .HasColumnName("Subtotal")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            subtotal.Property(m => m.Currency)
                .HasColumnName("SubtotalCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(i => i.Discount, discount =>
        {
            discount.Property(m => m.Amount)
                .HasColumnName("Discount")
                .HasColumnType("decimal(18,2)");

            discount.Property(m => m.Currency)
                .HasColumnName("DiscountCurrency")
                .HasMaxLength(3);
        });

        builder.OwnsOne(i => i.Total, total =>
        {
            total.Property(m => m.Amount)
                .HasColumnName("Total")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            total.Property(m => m.Currency)
                .HasColumnName("TotalCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(i => i.ImageUrl)
            .HasMaxLength(500);

        builder.HasIndex(i => i.OrderId);
        builder.HasIndex(i => i.ProductId);
    }
}