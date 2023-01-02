using ModularMonolith.BuildingBlocks.EventBus.Events;

namespace Webinar.API.IntegrationEvents.Events;

public record WebinarCreatedIntegrationEvent : IntegrationEvent
{
    public int WebinarId { get; }

    public WebinarCreatedIntegrationEvent(int webinarId) => WebinarId = webinarId;
}