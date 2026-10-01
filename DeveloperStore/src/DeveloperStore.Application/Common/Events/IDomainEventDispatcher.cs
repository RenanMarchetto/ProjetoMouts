using DeveloperStore.Domain.Common;

namespace DeveloperStore.Application.Common.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
    }
}
