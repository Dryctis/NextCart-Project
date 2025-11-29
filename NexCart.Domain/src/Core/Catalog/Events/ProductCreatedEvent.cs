using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Catalog.Events;

public sealed class ProductCreatedEvent : DomainEventBase
{
    public ProductId ProductId { get; }
    public string Name { get; }

    public ProductCreatedEvent(ProductId productId, string name)
    {
        ProductId = productId;
        Name = name;
    }
}