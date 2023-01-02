using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.BuildingBlocks.EventBus.Abstractions;
using ModularMonolith.BuildingBlocks.EventBus.Events;
using ModularMonolith.BuildingBlocks.EventBus.IntegrationEventLogEF.Services;
using ModularMonolith.BuildingBlocks.EventBus.IntegrationEventLogEF.Utilities;
using Webinar.API.Infrastructure;

namespace Webinar.API.IntegrationEvents;

public class WebinarIntegrationEventService : IWebinarIntegrationEventService, IDisposable
{
     private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
    private readonly IEventBus _eventBus;
    private readonly WebinarDbContext _webinarDbContext;
    private readonly IIntegrationEventLogService _eventLogService;
    private readonly ILogger<WebinarIntegrationEventService> _logger;
    private volatile bool disposedValue;

    public WebinarIntegrationEventService(
        ILogger<WebinarIntegrationEventService> logger,
        IEventBus eventBus,
        WebinarDbContext webinarDbContext,
        Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _webinarDbContext = webinarDbContext ?? throw new ArgumentNullException(nameof(webinarDbContext));
        _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _eventLogService = _integrationEventLogServiceFactory(_webinarDbContext.Database.GetDbConnection());
    }

    public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
    {
        try
        {
            _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

            await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
            _eventBus.Publish(evt);
            await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
            await _eventLogService.MarkEventAsFailedAsync(evt.Id);
        }
    }

    public async Task SaveIntegrationEventAndWebinarContextChangesAsync(IntegrationEvent evt)
    {
        _logger.LogInformation("----- CatalogIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

        //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
        await ResilientTransaction.New(_webinarDbContext).ExecuteAsync(async () =>
        {
            // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
            await _webinarDbContext.SaveChangesAsync();
            await _eventLogService.SaveEventAsync(evt, _webinarDbContext.Database.CurrentTransaction);
        });
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                (_eventLogService as IDisposable)?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}