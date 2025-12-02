using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Orders;
using NexCart.Domain.Tenants;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion(
                id => id.Value,
                value => OrderId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(o => o.TenantId)
            .HasConversion(
                id => id.Value,
                value => TenantId.Of(value))
            .IsRequired();

        builder.OwnsOne(o => o.OrderNumber, orderNumber =>
        {
            orderNumber.Property(on => on.Value)
                .HasColumnName("OrderNumber")
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.HasIndex(o => o.OrderNumber)
            .IsUnique();

        builder.Property(o => o.CustomerId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(o => o.CustomerEmail)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.PaymentStatus)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.FulfillmentType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(o => o.Subtotal, subtotal =>
        {
            subtotal.Property(m => m.Amount)
                .HasColumnName("Subtotal")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            subtotal.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(o => o.Discount, discount =>
        {
            discount.Property(m => m.Amount)
                .HasColumnName("Discount")
                .HasColumnType("decimal(18,2)");

            discount.Property(m => m.Currency)
                .HasColumnName("DiscountCurrency")
                .HasMaxLength(3);
        });

        builder.OwnsOne(o => o.ShippingCost, shippingCost =>
        {
            shippingCost.Property(m => m.Amount)
                .HasColumnName("ShippingCost")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            shippingCost.Property(m => m.Currency)
                .HasColumnName("ShippingCostCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(o => o.Tax, tax =>
        {
            tax.Property(m => m.Amount)
                .HasColumnName("Tax")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            tax.Property(m => m.Currency)
                .HasColumnName("TaxCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(o => o.Total, total =>
        {
            total.Property(m => m.Amount)
                .HasColumnName("Total")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            total.Property(m => m.Currency)
                .HasColumnName("TotalCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(o => o.ShippingInfo, shippingInfo =>
        {
            shippingInfo.Property(si => si.FullName)
                .HasColumnName("ShippingFullName")
                .HasMaxLength(100);

            shippingInfo.Property(si => si.Email)
                .HasColumnName("ShippingEmail")
                .HasMaxLength(255);

            shippingInfo.Property(si => si.Phone)
                .HasColumnName("ShippingPhone")
                .HasMaxLength(20);

            shippingInfo.OwnsOne(si => si.Address, address =>
            {
                address.Property(a => a.Street)
                    .HasColumnName("ShippingStreet")
                    .HasMaxLength(200);

                address.Property(a => a.City)
                    .HasColumnName("ShippingCity")
                    .HasMaxLength(100);

                address.Property(a => a.State)
                    .HasColumnName("ShippingState")
                    .HasMaxLength(100);

                address.Property(a => a.ZipCode)
                    .HasColumnName("ShippingZipCode")
                    .HasMaxLength(20);

                address.Property(a => a.Country)
                    .HasColumnName("ShippingCountry")
                    .HasMaxLength(100);
            });
        });

        builder.Property(o => o.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(o => o.PaymentTransactionId)
            .HasMaxLength(100);

        builder.OwnsOne(o => o.TrackingNumber, trackingNumber =>
        {
            trackingNumber.Property(tn => tn.Value)
                .HasColumnName("TrackingNumber")
                .HasMaxLength(100);
        });

        builder.Property(o => o.Notes)
            .HasMaxLength(1000);

        builder.Property(o => o.CancellationReason)
            .HasMaxLength(500);

        builder.Property(o => o.PaidAt);
        builder.Property(o => o.ShippedAt);
        builder.Property(o => o.DeliveredAt);
        builder.Property(o => o.CancelledAt);

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(o => o.UpdatedAt);

        builder.Property(o => o.UpdatedBy)
            .HasMaxLength(100);

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.TenantId);
        builder.HasIndex(o => o.CustomerId);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.PaymentStatus);
        builder.HasIndex(o => o.CreatedAt);

        builder.Ignore(o => o.DomainEvents);
    }
}