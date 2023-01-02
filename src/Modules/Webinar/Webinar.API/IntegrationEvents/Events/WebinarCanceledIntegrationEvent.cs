using ModularMonolith.BuildingBlocks.EventBus.Events;

namespace Webinar.API.IntegrationEvents.Events;

public record WebinarCanceledIntegrationEvent : IntegrationEvent
{
    public int WebinarId { get; }

    public WebinarCanceledIntegrationEvent(int webinarId) => WebinarId = webinarId;
}