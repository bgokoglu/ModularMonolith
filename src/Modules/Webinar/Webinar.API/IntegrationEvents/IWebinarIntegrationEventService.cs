using ModularMonolith.BuildingBlocks.EventBus.Events;

namespace Webinar.API.IntegrationEvents;

public interface IWebinarIntegrationEventService
{
    Task SaveIntegrationEventAndWebinarContextChangesAsync(IntegrationEvent evt);
    Task PublishThroughEventBusAsync(IntegrationEvent evt);
}