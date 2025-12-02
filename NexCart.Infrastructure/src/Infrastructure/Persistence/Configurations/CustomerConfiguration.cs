using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Customers;
using NexCart.Domain.Tenants;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CustomerId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(c => c.TenantId)
            .HasConversion(
                id => id.Value,
                value => TenantId.Of(value))
            .IsRequired();

        builder.Property(c => c.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.HasIndex(c => new { c.TenantId, c.Email })
            .IsUnique();

        builder.OwnsOne(c => c.Phone, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("Phone")
                .HasMaxLength(20);
        });

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(c => c.LoyaltyPoints, loyaltyPoints =>
        {
            loyaltyPoints.Property(lp => lp.Points)
                .HasColumnName("LoyaltyPoints")
                .IsRequired();
        });

        builder.Property(c => c.LastOrderDate);

        builder.Property(c => c.TotalOrders)
            .IsRequired();

        builder.OwnsOne(c => c.TotalSpent, totalSpent =>
        {
            totalSpent.Property(m => m.Amount)
                .HasColumnName("TotalSpent")
                .HasColumnType("decimal(18,2)");

            totalSpent.Property(m => m.Currency)
                .HasColumnName("TotalSpentCurrency")
                .HasMaxLength(3);
        });

        builder.Property(c => c.EmailVerified)
            .IsRequired();

        builder.Property(c => c.EmailVerifiedAt);

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

        builder.HasMany(c => c.Addresses)
            .WithOne()
            .HasForeignKey("CustomerId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => c.Status);

        builder.Ignore(c => c.DomainEvents);
    }
}