using NexCart.Domain.Common.Events;

namespace NexCart.Domain.Catalog.Events;

public sealed class InventoryUpdatedEvent : DomainEventBase
{
    public ProductId ProductId { get; }
    public int OldQuantity { get; }
    public int NewQuantity { get; }

    public InventoryUpdatedEvent(ProductId productId, int oldQuantity, int newQuantity)
    {
        ProductId = productId;
        OldQuantity = oldQuantity;
        NewQuantity = newQuantity;
    }
}