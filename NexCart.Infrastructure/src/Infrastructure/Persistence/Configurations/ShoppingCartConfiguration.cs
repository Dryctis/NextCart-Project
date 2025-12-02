using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Shopping;
using NexCart.Domain.Tenants;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.ToTable("ShoppingCarts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => ShoppingCartId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(c => c.TenantId)
            .HasConversion(
                id => id.Value,
                value => TenantId.Of(value))
            .IsRequired();

        builder.Property(c => c.CustomerId)
            .HasMaxLength(100);

        builder.Property(c => c.CustomerEmail)
            .HasMaxLength(255);

        builder.Property(c => c.SessionId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(c => c.Total, total =>
        {
            total.Property(m => m.Amount)
                .HasColumnName("Total")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            total.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(c => c.TotalItems)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.Property(c => c.AbandonedAt);

        builder.Property(c => c.ConvertedAt);

        builder.Property(c => c.ExpiresAt)
            .IsRequired();

        builder.HasMany(c => c.Items)
            .WithOne()
            .HasForeignKey("ShoppingCartId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => c.CustomerId);
        builder.HasIndex(c => c.SessionId);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.ExpiresAt);

        builder.Ignore(c => c.DomainEvents);
    }
}