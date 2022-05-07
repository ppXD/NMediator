using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Events;

namespace NMediator.Test.TestData.EventHandlers;

public class TestEventHandler : IEventHandler<TestEvent>
{
    public Task Handle(TestEvent @event, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(@event);
        return Task.CompletedTask;
    }
}
    
public class TestEventHandler1 : IEventHandler<TestEvent>
{
    public Task Handle(TestEvent @event, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(@event);
        return Task.CompletedTask;
    }
}