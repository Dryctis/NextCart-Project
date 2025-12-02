using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Shopping;
using NexCart.Domain.Tenants;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.ToTable("Wishlists");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasConversion(
                id => id.Value,
                value => WishlistId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(w => w.TenantId)
            .HasConversion(
                id => id.Value,
                value => TenantId.Of(value))
            .IsRequired();

        builder.Property(w => w.CustomerId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(w => w.Name)
            .HasMaxLength(100);

        builder.Property(w => w.IsPublic)
            .IsRequired();

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.UpdatedAt)
            .IsRequired();

        builder.HasMany(w => w.Items)
            .WithOne()
            .HasForeignKey("WishlistId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(w => w.TenantId);
        builder.HasIndex(w => w.CustomerId);

        builder.Ignore(w => w.DomainEvents);
    }
}