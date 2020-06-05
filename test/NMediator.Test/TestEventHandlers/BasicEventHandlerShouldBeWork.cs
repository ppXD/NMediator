using System.Threading.Tasks;
using NMediator.Test.TestData;
using NMediator.Test.TestData.EventHandlers;
using NMediator.Test.TestData.Events;
using Shouldly;
using Xunit;

namespace NMediator.Test.TestEventHandlers
{
    public class BasicEventHandlerShouldBeWork
    {
        [Fact]
        public async Task Test()
        {
            var mediator = new MediatorConfiguration()
                .RegisterHandlers(typeof(TestEvent).Assembly)
                .CreateMediator();

            await mediator.PublishAsync(new TestEvent
            {
                Message = "test event"
            });
            
            TestStore.EventStore.Count.ShouldBe(2);
        }
    }
}