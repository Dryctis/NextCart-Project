using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Catalog;
using NexCart.Domain.Shopping;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => CartItemId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(i => i.ShoppingCartId)
            .HasConversion(
                id => id.Value,
                value => ShoppingCartId.Of(value))
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

        builder.Property(i => i.ImageUrl)
            .HasMaxLength(500);

        builder.Property(i => i.AddedAt)
            .IsRequired();

        builder.Property(i => i.UpdatedAt);

        builder.HasIndex(i => i.ShoppingCartId);
        builder.HasIndex(i => i.ProductId);
    }
}