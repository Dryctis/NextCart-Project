using NexCart.Domain.Common.Events;
using NexCart.Domain.Common.ValueObjects;

namespace NexCart.Domain.Catalog.Events;

public sealed class ProductPriceChangedEvent : DomainEventBase
{
    public ProductId ProductId { get; }
    public Money OldPrice { get; }
    public Money NewPrice { get; }

    public ProductPriceChangedEvent(ProductId productId, Money oldPrice, Money newPrice)
    {
        ProductId = productId;
        OldPrice = oldPrice;
        NewPrice = newPrice;
    }
}