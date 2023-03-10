using ModularMonolith.BuildingBlocks.EventBus.Abstractions;

namespace ModularMonolith.BuildingBlocks.EventBus.Tests;

public class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
{
    public bool Handled { get; private set; }

    public TestIntegrationEventHandler()
    {
        Handled = false;
    }

    public async Task Handle(TestIntegrationEvent @event)
    {
        Handled = true;
    }
}