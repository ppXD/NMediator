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

    [Fact]
    public async Task ShouldEventHasMultipleHandlers()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestEventHandler>()
            .RegisterHandler<TestEventHandler1>()
            .CreateMediator();

        var @event = new TestEvent();
        
        await mediator.PublishAsync(@event);
        await mediator.PublishAsync(@event);
        
        TestStore.Stores.Count.ShouldBe(4);
    }

    [Fact]
    public async Task ShouldEventPublishToItsHandler()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestEventHandler>()
            .RegisterHandler<TestEventHandler1>()
            .RegisterHandler<TestMultipleEventHandler>()
            .CreateMediator();

        var @event = new TestMultipleEvent1();
        
        await mediator.PublishAsync(@event);
        
        TestStore.Stores.Count.ShouldBe(1);
        TestStore.Stores.Single().ShouldBe(@event);
    }
    
    [Fact]
    public async Task ShouldOneHandlerToHandleMultipleEvents()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestMultipleEventHandler>()
            .CreateMediator();

        var event1 = new TestMultipleEvent1();
        var event2 = new TestMultipleEvent2();
        
        await mediator.PublishAsync(event1);
        await mediator.PublishAsync(event2);
        
        TestStore.Stores.Count.ShouldBe(2);
        TestStore.Stores.Any(x => x.GetType() == typeof(TestMultipleEvent1)).ShouldBeTrue();
        TestStore.Stores.Any(x => x.GetType() == typeof(TestMultipleEvent2)).ShouldBeTrue();
    }
}