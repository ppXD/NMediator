using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Events;

namespace NMediator.Test.TestData.EventHandlers
{
    public class TestEventHandler : IEventHandler<TestEvent>
    {
        public Task Handle(IMessageContext<TestEvent> context, CancellationToken cancellationToken = default)
        {
            TestStore.EventStore.Add(context.Message);

            return Task.CompletedTask;
        }
    }
    
    public class TestEventHandler1 : IEventHandler<TestEvent>
    {
        public Task Handle(IMessageContext<TestEvent> context, CancellationToken cancellationToken = default)
        {
            TestStore.EventStore.Add(context.Message);

            return Task.CompletedTask;
        }
    }
}