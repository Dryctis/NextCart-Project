using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Catalog;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => ProductImageId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(i => i.ProductId)
            .HasConversion(
                id => id.Value,
                value => ProductId.Of(value))
            .IsRequired();

        builder.Property(i => i.Url)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(i => i.AltText)
            .HasMaxLength(200);

        builder.Property(i => i.DisplayOrder)
            .IsRequired();

        builder.Property(i => i.IsPrimary)
            .IsRequired();

        builder.HasIndex(i => i.ProductId);
        builder.HasIndex(i => new { i.ProductId, i.IsPrimary });
    }
}