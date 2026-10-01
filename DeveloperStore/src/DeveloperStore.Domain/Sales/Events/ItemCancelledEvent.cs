using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.Sales.Events
{
    public sealed record ItemCancelledEvent(Guid SaleId, Guid ItemId, DateTime OccurredOn) : IDomainEvent;
}
