using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Catalog;
using NexCart.Domain.Tenants;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CategoryId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(c => c.TenantId)
            .HasConversion(
                id => id.Value,
                value => TenantId.Of(value))
            .IsRequired();

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.Slug)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(c => new { c.TenantId, c.Slug })
            .IsUnique();

        builder.Property(c => c.ParentCategoryId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? CategoryId.Of(value.Value) : null);

        builder.Property(c => c.DisplayOrder)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.Property(c => c.ImageUrl)
            .HasMaxLength(500);

        builder.Property(c => c.IconUrl)
            .HasMaxLength(500);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.Property(c => c.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(c => c.IsDeleted)
            .IsRequired();

        builder.Property(c => c.DeletedAt);

        builder.Property(c => c.DeletedBy)
            .HasMaxLength(100);

        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasIndex(c => c.TenantId);
    }
}