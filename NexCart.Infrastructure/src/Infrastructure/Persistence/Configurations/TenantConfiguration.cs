using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Tenants;

namespace NexCart.Infrastructure.Persistence.Configurations;

public sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => TenantId.Of(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Slug)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(t => t.Slug)
            .IsUnique();

        builder.OwnsOne(t => t.OwnerEmail, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("OwnerEmail")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.Property(t => t.OwnerName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Vertical)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Plan)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(t => t.Settings, settings =>
        {
            settings.Property(s => s.EnableVariants)
                .HasColumnName("Settings_EnableVariants");

            settings.Property(s => s.EnableModifiers)
                .HasColumnName("Settings_EnableModifiers");

            settings.Property(s => s.EnableDeliveryZones)
                .HasColumnName("Settings_EnableDeliveryZones");

            settings.Property(s => s.EnableTimeSlots)
                .HasColumnName("Settings_EnableTimeSlots");

            settings.Property(s => s.EnablePrescriptions)
                .HasColumnName("Settings_EnablePrescriptions");

            settings.Property(s => s.EnableQuoteRequests)
                .HasColumnName("Settings_EnableQuoteRequests");

            settings.Property(s => s.EnableVolumePricing)
                .HasColumnName("Settings_EnableVolumePricing");

            settings.Property(s => s.EnableMultiVendor)
                .HasColumnName("Settings_EnableMultiVendor");

            settings.Property(s => s.EnableSubscriptions)
                .HasColumnName("Settings_EnableSubscriptions");

            settings.Property(s => s.EnableReviews)
                .HasColumnName("Settings_EnableReviews");

            settings.Property(s => s.EnableWishlist)
                .HasColumnName("Settings_EnableWishlist");

            settings.Property(s => s.EnableLoyaltyPoints)
                .HasColumnName("Settings_EnableLoyaltyPoints");

            settings.Property(s => s.AllowGuestCheckout)
                .HasColumnName("Settings_AllowGuestCheckout");

            settings.Property(s => s.RequireEmailVerification)
                .HasColumnName("Settings_RequireEmailVerification");

            settings.Property(s => s.MinimumOrderAmount)
                .HasColumnName("Settings_MinimumOrderAmount")
                .HasColumnType("decimal(18,2)");

            settings.Property(s => s.StoreName)
                .HasColumnName("Settings_StoreName")
                .HasMaxLength(200);

            settings.Property(s => s.StoreDescription)
                .HasColumnName("Settings_StoreDescription")
                .HasMaxLength(1000);

            settings.Property(s => s.DefaultCurrency)
                .HasColumnName("Settings_DefaultCurrency")
                .HasMaxLength(3);

            settings.Property(s => s.DefaultLanguage)
                .HasColumnName("Settings_DefaultLanguage")
                .HasMaxLength(10);

            settings.Property(s => s.TimeZone)
                .HasColumnName("Settings_TimeZone")
                .HasMaxLength(50);
        });

        builder.Property(t => t.TrialEndsAt);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.UpdatedAt);

        builder.Property(t => t.UpdatedBy)
            .HasMaxLength(100);

        builder.Ignore(t => t.DomainEvents);
    }
}