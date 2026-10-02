using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.Sales.Events
{
    public sealed record SaleModifiedEvent(Guid SaleId, DateTime OccurredOn) : IDomainEvent;
}
