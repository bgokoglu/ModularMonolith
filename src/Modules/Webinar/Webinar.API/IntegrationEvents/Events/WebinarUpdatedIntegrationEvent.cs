using ModularMonolith.BuildingBlocks.EventBus.Events;

namespace Webinar.API.IntegrationEvents.Events;

public record WebinarUpdatedIntegrationEvent : IntegrationEvent
{
    public int WebinarId { get; }

    public WebinarUpdatedIntegrationEvent(int webinarId) => WebinarId = webinarId;
}