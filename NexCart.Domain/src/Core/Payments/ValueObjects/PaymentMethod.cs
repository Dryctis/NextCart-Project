using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Payments.ValueObjects;

public sealed class PaymentMethod : ValueObject
{
    public string Type { get; }
    public string? Last4Digits { get; }
    public string? CardBrand { get; }
    public string? CardholderName { get; }

    private PaymentMethod(string type, string? last4Digits, string? cardBrand, string? cardholderName)
    {
        Type = type;
        Last4Digits = last4Digits;
        CardBrand = cardBrand;
        CardholderName = cardholderName;
    }

    public static PaymentMethod Card(string last4Digits, string cardBrand, string cardholderName)
    {
        if (string.IsNullOrWhiteSpace(last4Digits) || last4Digits.Length != 4)
            throw new ArgumentException("Los últimos 4 dígitos son inválidos", nameof(last4Digits));

        if (string.IsNullOrWhiteSpace(cardBrand))
            throw new ArgumentException("La marca de la tarjeta es requerida", nameof(cardBrand));

        return new PaymentMethod("card", last4Digits, cardBrand.Trim(), cardholderName?.Trim());
    }

    public static PaymentMethod Cash()
    {
        return new PaymentMethod("cash", null, null, null);
    }

    public static PaymentMethod BankTransfer()
    {
        return new PaymentMethod("bank_transfer", null, null, null);
    }

    public static PaymentMethod PayPal(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email de PayPal es requerido", nameof(email));

        return new PaymentMethod("paypal", null, null, email.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return Last4Digits;
        yield return CardBrand;
        yield return CardholderName;
    }

    public override string ToString()
    {
        return Type switch
        {
            "card" => $"{CardBrand} ****{Last4Digits}",
            "paypal" => $"PayPal ({CardholderName})",
            _ => Type
        };
    }
}