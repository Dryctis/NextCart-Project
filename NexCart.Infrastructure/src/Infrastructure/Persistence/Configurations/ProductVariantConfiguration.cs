using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Catalog;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => ProductVariantId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(v => v.ProductId)
            .HasConversion(
                id => id.Value,
                value => ProductId.Of(value))
            .IsRequired();

        builder.Property(v => v.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(v => v.Sku, sku =>
        {
            sku.Property(s => s.Value)
                .HasColumnName("Sku")
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.OwnsOne(v => v.Price, price =>
        {
            price.Property(m => m.Amount)
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            price.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(v => v.CompareAtPrice, comparePrice =>
        {
            comparePrice.Property(m => m.Amount)
                .HasColumnName("CompareAtPrice")
                .HasColumnType("decimal(18,2)");

            comparePrice.Property(m => m.Currency)
                .HasColumnName("CompareAtPriceCurrency")
                .HasMaxLength(3);
        });

        builder.Property(v => v.StockQuantity)
            .IsRequired();

        builder.OwnsOne(v => v.Weight, weight =>
        {
            weight.Property(w => w.Value)
                .HasColumnName("Weight")
                .HasColumnType("decimal(18,3)");

            weight.Property(w => w.Unit)
                .HasColumnName("WeightUnit")
                .HasMaxLength(10);
        });

        builder.Property(v => v.ImageUrl)
            .HasMaxLength(500);

        builder.Property(v => v.IsActive)
            .IsRequired();

        builder.Property(v => v.DisplayOrder)
            .IsRequired();

        builder.HasIndex(v => v.ProductId);
        builder.HasIndex(v => v.Sku);
    }
}