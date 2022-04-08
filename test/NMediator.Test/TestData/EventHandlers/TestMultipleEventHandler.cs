using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Events;

namespace NMediator.Test.TestData.EventHandlers;

public class TestMultipleEventHandler : IEventHandler<TestMultipleEvent1>, IEventHandler<TestMultipleEvent2>
{
    public Task Handle(IEventContext<TestMultipleEvent1> context, CancellationToken cancellationToken = default)
    {
        TestStore.EventStore.Add(context.Message);
            
        return Task.CompletedTask;
    }

    public Task Handle(IEventContext<TestMultipleEvent2> context, CancellationToken cancellationToken = default)
    {
        TestStore.EventStore.Add(context.Message);
            
        return Task.CompletedTask;
    }
}