using DeveloperStore.Domain.Common;
using Microsoft.Extensions.Logging;


namespace DeveloperStore.Application.Common.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(ILogger<DomainEventDispatcher> logger)
        {
            _logger = logger;
        }

        public Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
        {
            foreach (var domainEvent in events)
            {
                _logger.LogInformation("Dispatching domain event: {EventName} at {OccurredOn}", domainEvent.GetType().Name, domainEvent.OccurredOn);
            }
            return Task.CompletedTask;
        }
    }
}
