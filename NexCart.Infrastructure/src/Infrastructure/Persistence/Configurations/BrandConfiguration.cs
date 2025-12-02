using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Catalog;
using NexCart.Domain.Tenants;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => BrandId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(b => b.TenantId)
            .HasConversion(
                id => id.Value,
                value => TenantId.Of(value))
            .IsRequired();

        builder.Property(b => b.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.Description)
            .HasMaxLength(500);

        builder.Property(b => b.Slug)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(b => new { b.TenantId, b.Slug })
            .IsUnique();

        builder.Property(b => b.LogoUrl)
            .HasMaxLength(500);

        builder.Property(b => b.WebsiteUrl)
            .HasMaxLength(500);

        builder.Property(b => b.IsActive)
            .IsRequired();

        builder.Property(b => b.DisplayOrder)
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.UpdatedAt);

        builder.Property(b => b.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(b => b.IsDeleted)
            .IsRequired();

        builder.Property(b => b.DeletedAt);

        builder.Property(b => b.DeletedBy)
            .HasMaxLength(100);

        builder.HasQueryFilter(b => !b.IsDeleted);

        builder.HasIndex(b => b.TenantId);
    }
}