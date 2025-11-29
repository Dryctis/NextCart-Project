using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Tenants;

public sealed class TenantSettings : ValueObject
{
    public bool EnableVariants { get; private set; }
    public bool EnableModifiers { get; private set; }
    public bool EnableDeliveryZones { get; private set; }
    public bool EnableTimeSlots { get; private set; }
    public bool EnablePrescriptions { get; private set; }
    public bool EnableQuoteRequests { get; private set; }
    public bool EnableVolumePricing { get; private set; }
    public bool EnableMultiVendor { get; private set; }
    public bool EnableSubscriptions { get; private set; }
    public bool EnableReviews { get; private set; }
    public bool EnableWishlist { get; private set; }
    public bool EnableLoyaltyPoints { get; private set; }

    public bool AllowGuestCheckout { get; private set; }
    public bool RequireEmailVerification { get; private set; }
    public decimal MinimumOrderAmount { get; private set; }

    public string StoreName { get; private set; }
    public string StoreDescription { get; private set; }
    public string DefaultCurrency { get; private set; }
    public string DefaultLanguage { get; private set; }
    public string TimeZone { get; private set; }

    private TenantSettings()
    {
        StoreName = string.Empty;
        StoreDescription = string.Empty;
        DefaultCurrency = string.Empty;
        DefaultLanguage = string.Empty;
        TimeZone = string.Empty;
    }

    public static TenantSettings CreateDefault(BusinessVertical vertical)
    {
        return vertical switch
        {
            BusinessVertical.Retail => CreateRetailDefaults(),
            BusinessVertical.FoodAndBeverage => CreateFoodDefaults(),
            BusinessVertical.Services => CreateServicesDefaults(),
            BusinessVertical.Pharmacy => CreatePharmacyDefaults(),
            BusinessVertical.B2BWholesale => CreateB2BDefaults(),
            BusinessVertical.DigitalProducts => CreateDigitalDefaults(),
            BusinessVertical.Marketplace => CreateMarketplaceDefaults(),
            _ => CreateRetailDefaults()
        };
    }

    private static TenantSettings CreateRetailDefaults()
    {
        return new TenantSettings
        {
            EnableVariants = true,
            EnableModifiers = false,
            EnableDeliveryZones = false,
            EnableTimeSlots = false,
            EnablePrescriptions = false,
            EnableQuoteRequests = false,
            EnableVolumePricing = false,
            EnableMultiVendor = false,
            EnableSubscriptions = false,
            EnableReviews = true,
            EnableWishlist = true,
            EnableLoyaltyPoints = true,
            AllowGuestCheckout = true,
            RequireEmailVerification = false,
            MinimumOrderAmount = 0,
            DefaultCurrency = "USD",
            DefaultLanguage = "es",
            TimeZone = "UTC",
            StoreName = "Mi Tienda",
            StoreDescription = string.Empty
        };
    }

    private static TenantSettings CreateFoodDefaults()
    {
        return new TenantSettings
        {
            EnableVariants = false,
            EnableModifiers = true,
            EnableDeliveryZones = true,
            EnableTimeSlots = true,
            EnablePrescriptions = false,
            EnableQuoteRequests = false,
            EnableVolumePricing = false,
            EnableMultiVendor = false,
            EnableSubscriptions = false,
            EnableReviews = true,
            EnableWishlist = false,
            EnableLoyaltyPoints = true,
            AllowGuestCheckout = true,
            RequireEmailVerification = false,
            MinimumOrderAmount = 10,
            DefaultCurrency = "USD",
            DefaultLanguage = "es",
            TimeZone = "UTC",
            StoreName = "Mi Restaurante",
            StoreDescription = string.Empty
        };
    }

    private static TenantSettings CreateServicesDefaults()
    {
        return new TenantSettings
        {
            EnableVariants = false,
            EnableModifiers = true,
            EnableDeliveryZones = false,
            EnableTimeSlots = true,
            EnablePrescriptions = false,
            EnableQuoteRequests = true,
            EnableVolumePricing = false,
            EnableMultiVendor = false,
            EnableSubscriptions = true,
            EnableReviews = true,
            EnableWishlist = false,
            EnableLoyaltyPoints = false,
            AllowGuestCheckout = false,
            RequireEmailVerification = true,
            MinimumOrderAmount = 0,
            DefaultCurrency = "USD",
            DefaultLanguage = "es",
            TimeZone = "UTC",
            StoreName = "Mi Servicio",
            StoreDescription = string.Empty
        };
    }

    private static TenantSettings CreatePharmacyDefaults()
    {
        return new TenantSettings
        {
            EnableVariants = true,
            EnableModifiers = false,
            EnableDeliveryZones = true,
            EnableTimeSlots = true,
            EnablePrescriptions = true,
            EnableQuoteRequests = false,
            EnableVolumePricing = false,
            EnableMultiVendor = false,
            EnableSubscriptions = true,
            EnableReviews = false,
            EnableWishlist = false,
            EnableLoyaltyPoints = true,
            AllowGuestCheckout = false,
            RequireEmailVerification = true,
            MinimumOrderAmount = 0,
            DefaultCurrency = "USD",
            DefaultLanguage = "es",
            TimeZone = "UTC",
            StoreName = "Mi Farmacia",
            StoreDescription = string.Empty
        };
    }

    private static TenantSettings CreateB2BDefaults()
    {
        return new TenantSettings
        {
            EnableVariants = true,
            EnableModifiers = false,
            EnableDeliveryZones = false,
            EnableTimeSlots = false,
            EnablePrescriptions = false,
            EnableQuoteRequests = true,
            EnableVolumePricing = true,
            EnableMultiVendor = false,
            EnableSubscriptions = false,
            EnableReviews = false,
            EnableWishlist = false,
            EnableLoyaltyPoints = false,
            AllowGuestCheckout = false,
            RequireEmailVerification = true,
            MinimumOrderAmount = 100,
            DefaultCurrency = "USD",
            DefaultLanguage = "es",
            TimeZone = "UTC",
            StoreName = "Mi Tienda B2B",
            StoreDescription = string.Empty
        };
    }

    private static TenantSettings CreateDigitalDefaults()
    {
        return new TenantSettings
        {
            EnableVariants = false,
            EnableModifiers = false,
            EnableDeliveryZones = false,
            EnableTimeSlots = false,
            EnablePrescriptions = false,
            EnableQuoteRequests = false,
            EnableVolumePricing = true,
            EnableMultiVendor = false,
            EnableSubscriptions = true,
            EnableReviews = true,
            EnableWishlist = true,
            EnableLoyaltyPoints = false,
            AllowGuestCheckout = true,
            RequireEmailVerification = false,
            MinimumOrderAmount = 0,
            DefaultCurrency = "USD",
            DefaultLanguage = "es",
            TimeZone = "UTC",
            StoreName = "Mi Tienda Digital",
            StoreDescription = string.Empty
        };
    }

    private static TenantSettings CreateMarketplaceDefaults()
    {
        return new TenantSettings
        {
            EnableVariants = true,
            EnableModifiers = false,
            EnableDeliveryZones = true,
            EnableTimeSlots = false,
            EnablePrescriptions = false,
            EnableQuoteRequests = false,
            EnableVolumePricing = false,
            EnableMultiVendor = true,
            EnableSubscriptions = false,
            EnableReviews = true,
            EnableWishlist = true,
            EnableLoyaltyPoints = true,
            AllowGuestCheckout = true,
            RequireEmailVerification = false,
            MinimumOrderAmount = 0,
            DefaultCurrency = "USD",
            DefaultLanguage = "es",
            TimeZone = "UTC",
            StoreName = "Mi Marketplace",
            StoreDescription = string.Empty
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EnableVariants;
        yield return EnableModifiers;
        yield return EnableDeliveryZones;
        yield return EnableTimeSlots;
        yield return AllowGuestCheckout;
        yield return MinimumOrderAmount;
        yield return DefaultCurrency;
    }
}