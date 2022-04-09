using System.Linq;
using System.Threading.Tasks;
using NMediator.Test.TestData;
using NMediator.Test.TestData.EventHandlers;
using NMediator.Test.TestData.Events;
using Shouldly;
using Xunit;

namespace NMediator.Test;

public class EventFixture : TestBase
{
    [Fact]
    public async Task ShouldEventHandled()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestEventHandler>()
            .CreateMediator();

        var @event = new TestEvent();
        
        await mediator.PublishAsync(@event);
        
        TestStore.Stores.Count.ShouldBe(1);
        TestStore.Stores.Single().ShouldBe(@event);
    }
}