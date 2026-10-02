using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.Sales.Events
{
    public sealed record SaleCancelledEvent(Guid SaleId, DateTime OccurredOn) : IDomainEvent;
}
