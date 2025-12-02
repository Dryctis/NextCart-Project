using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Catalog;
using NexCart.Domain.Tenants;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProductId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(p => p.TenantId)
            .HasConversion(
                id => id.Value,
                value => TenantId.Of(value))
            .IsRequired();

        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.OwnsOne(p => p.Sku, sku =>
        {
            sku.Property(s => s.Value)
                .HasColumnName("Sku")
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.HasIndex(p => new { p.TenantId, p.Sku })
            .IsUnique();

        builder.OwnsOne(p => p.Price, price =>
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

        builder.OwnsOne(p => p.CompareAtPrice, comparePrice =>
        {
            comparePrice.Property(m => m.Amount)
                .HasColumnName("CompareAtPrice")
                .HasColumnType("decimal(18,2)");

            comparePrice.Property(m => m.Currency)
                .HasColumnName("CompareAtPriceCurrency")
                .HasMaxLength(3);
        });

        builder.OwnsOne(p => p.Cost, cost =>
        {
            cost.Property(m => m.Amount)
                .HasColumnName("Cost")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            cost.Property(m => m.Currency)
                .HasColumnName("CostCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(p => p.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.CategoryId)
            .HasConversion(
                id => id.Value,
                value => CategoryId.Of(value))
            .IsRequired();

        builder.Property(p => p.BrandId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? BrandId.Of(value.Value) : null);

        builder.Property(p => p.StockQuantity)
            .IsRequired();

        builder.Property(p => p.LowStockThreshold)
            .IsRequired();

        builder.Property(p => p.TrackInventory)
            .IsRequired();

        builder.OwnsOne(p => p.Weight, weight =>
        {
            weight.Property(w => w.Value)
                .HasColumnName("Weight")
                .HasColumnType("decimal(18,3)");

            weight.Property(w => w.Unit)
                .HasColumnName("WeightUnit")
                .HasMaxLength(10);
        });

        builder.OwnsOne(p => p.Dimensions, dimensions =>
        {
            dimensions.Property(d => d.Length)
                .HasColumnName("DimensionLength")
                .HasColumnType("decimal(18,2)");

            dimensions.Property(d => d.Width)
                .HasColumnName("DimensionWidth")
                .HasColumnType("decimal(18,2)");

            dimensions.Property(d => d.Height)
                .HasColumnName("DimensionHeight")
                .HasColumnType("decimal(18,2)");

            dimensions.Property(d => d.Unit)
                .HasColumnName("DimensionUnit")
                .HasMaxLength(10);
        });

        builder.OwnsOne(p => p.Rating, rating =>
        {
            rating.Property(r => r.Value)
                .HasColumnName("RatingValue")
                .HasColumnType("decimal(3,2)")
                .IsRequired();

            rating.Property(r => r.ReviewCount)
                .HasColumnName("ReviewCount")
                .IsRequired();
        });

        builder.Property(p => p.IsFeatured)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);

        builder.Property(p => p.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(p => p.IsDeleted)
            .IsRequired();

        builder.Property(p => p.DeletedAt);

        builder.Property(p => p.DeletedBy)
            .HasMaxLength(100);

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.HasIndex(p => p.TenantId);
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.BrandId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.IsFeatured);

        builder.Ignore(p => p.DomainEvents);
    }
}