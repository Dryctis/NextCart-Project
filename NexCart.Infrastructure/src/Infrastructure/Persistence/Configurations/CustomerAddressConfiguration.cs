using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Customers;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable("CustomerAddresses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => CustomerAddressId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(a => a.CustomerId)
            .HasConversion(
                id => id.Value,
                value => CustomerId.Of(value))
            .IsRequired();

        builder.Property(a => a.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(a => a.Phone, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("Phone")
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.OwnsOne(a => a.Address, address =>
        {
            address.Property(addr => addr.Street)
                .HasColumnName("Street")
                .HasMaxLength(200)
                .IsRequired();

            address.Property(addr => addr.City)
                .HasColumnName("City")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(addr => addr.State)
                .HasColumnName("State")
                .HasMaxLength(100);

            address.Property(addr => addr.ZipCode)
                .HasColumnName("ZipCode")
                .HasMaxLength(20);

            address.Property(addr => addr.Country)
                .HasColumnName("Country")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(a => a.IsDefault)
            .IsRequired();

        builder.Property(a => a.Label)
            .HasMaxLength(50);

        builder.HasIndex(a => a.CustomerId);
        builder.HasIndex(a => new { a.CustomerId, a.IsDefault });
    }
}