using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Catalog;
using NexCart.Domain.Shopping;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
{
    public void Configure(EntityTypeBuilder<WishlistItem> builder)
    {
        builder.ToTable("WishlistItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => WishlistItemId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(i => i.WishlistId)
            .HasConversion(
                id => id.Value,
                value => WishlistId.Of(value))
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

        builder.Property(i => i.AddedAt)
            .IsRequired();

        builder.HasIndex(i => i.WishlistId);
        builder.HasIndex(i => i.ProductId);
    }
}