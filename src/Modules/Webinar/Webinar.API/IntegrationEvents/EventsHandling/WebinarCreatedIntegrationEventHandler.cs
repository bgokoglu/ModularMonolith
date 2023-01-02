using ModularMonolith.BuildingBlocks.EventBus.Abstractions;
using Serilog.Context;
using Webinar.API.Infrastructure;
using Webinar.API.IntegrationEvents.Events;

namespace Webinar.API.IntegrationEvents.EventsHandling;

public class WebinarCreatedIntegrationEventHandler : IIntegrationEventHandler<WebinarCreatedIntegrationEvent>
{
    private readonly WebinarDbContext _webinarDbContext;
    private readonly ILogger<WebinarCreatedIntegrationEventHandler> _logger;

    public WebinarCreatedIntegrationEventHandler(
        WebinarDbContext webinarDbContext,
        ILogger<WebinarCreatedIntegrationEventHandler> logger)
    {
        _webinarDbContext = webinarDbContext;
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
    }

    public async Task Handle(WebinarCreatedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
        {
            _logger.LogInformation(
                "----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})",
                @event.Id, Program.AppName, @event);

            //do work
            //send emails, sms, etc, schedule background jobs

            await _webinarDbContext.SaveChangesAsync();
        }
    }
}