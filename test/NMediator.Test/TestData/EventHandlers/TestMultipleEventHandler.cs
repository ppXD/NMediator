using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Events;

namespace NMediator.Test.TestData.EventHandlers
{
    public class TestMultipleEventHandler : IEventHandler<TestMultipleEvent1>, IEventHandler<TestMultipleEvent2>
    {
        public Task Handle(TestMultipleEvent1 @event, CancellationToken cancellationToken)
        {
            TestStore.EventStore.Add(@event);
            
            return Task.CompletedTask;
        }

        public Task Handle(TestMultipleEvent2 @event, CancellationToken cancellationToken)
        {
            TestStore.EventStore.Add(@event);
            
            return Task.CompletedTask;
        }
    }
}