using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.Sales.Events
{
    public sealed record SaleCreatedEvent(Guid SaleId, string SaleNumber, DateTime OccurredOn) : IDomainEvent;

}
