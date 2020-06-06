using System.Threading.Tasks;
using NMediator.Test.TestData;
using NMediator.Test.TestData.Events;
using Shouldly;
using Xunit;

namespace NMediator.Test.TestEventHandlers
{
    public class MultipleEventHandlerShouldBeWork : TestBase
    {
        [Fact]
        public async Task Test()
        {
            var mediator = new MediatorConfiguration()
                .RegisterHandlers(typeof(TestMultipleEvent1).Assembly)
                .CreateMediator();

            await mediator.PublishAsync(new TestMultipleEvent1());
            
            TestStore.EventStore.Count.ShouldBe(1);
            
            await mediator.PublishAsync(new TestMultipleEvent2());
            
            TestStore.EventStore.Count.ShouldBe(2);
        }
    }
}