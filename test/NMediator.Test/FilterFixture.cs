using System;
using System.Linq;
using System.Threading.Tasks;
using NMediator.Test.TestData;
using NMediator.Test.TestData.CommandHandlers;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.EventHandlers;
using NMediator.Test.TestData.Events;
using NMediator.Test.TestData.Filters.CommandFilters;
using NMediator.Test.TestData.Filters.EventFilters;
using NMediator.Test.TestData.Filters.MessageFilters;
using NMediator.Test.TestData.Filters.RequestFilters;
using NMediator.Test.TestData.RequestHandlers;
using NMediator.Test.TestData.Requests;
using Shouldly;
using Xunit;

namespace NMediator.Test;

public class FilterFixture : TestBase
{
    [Fact]
    public async Task ShouldFilterInvoked()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseFilter<AllMessagesFilter1>()
            .CreateMediator();

        var command = new TestCommand(Guid.NewGuid());
        
        await mediator.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(3);
    }

    [Fact]
    public async Task ShouldInvokeMatchingFilters()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .RegisterHandler<TestOtherCommandHandler>()
            .RegisterHandler<TestRequestHandler>()
            .RegisterHandler<TestOtherRequestHandler>()
            .RegisterHandler<TestEventHandler>()
            .UseFilter<AllMessagesFilter1>()
            .UseFilter<AllMessagesFilter2>()
            .UseFilter<AllCommandsFilter1>()
            .UseFilter<AllCommandsFilter2>()
            .UseFilter<AllRequestsFilter1>()
            .UseFilter<AllEventsFilter1>()
            .UseFilter<AllEventsFilter2>()
            .UseFilter(typeof(TestCommandFilter))
            .UseFilter(typeof(TestOtherCommandFilter))
            .UseFilter(typeof(TestRequestFilter))
            .CreateMediator();
        
        var testCommand = new TestCommand(Guid.NewGuid());
        var otherCommand = new TestOtherCommand();
        
        await mediator.SendAsync(testCommand);
        TestStore.Stores.Count.ShouldBe(11);
        TestStore.Stores.Any(x => x.Equals($"{nameof(TestCommandFilter)} {nameof(TestCommandFilter.OnExecuting)}")).ShouldBeTrue();
        TestStore.Stores.Clear();
        
        await mediator.SendAsync(otherCommand);
        TestStore.Stores.Count.ShouldBe(11);
        TestStore.Stores.Any(x => x.Equals($"{nameof(TestOtherCommandFilter)} {nameof(TestOtherCommandFilter.OnExecuting)}")).ShouldBeTrue();
        TestStore.Stores.Clear();

        var response = await mediator.RequestAsync<TestRequest, TestResponse>(new TestRequest());
        response.ShouldNotBeNull();
        TestStore.Stores.Count.ShouldBe(9);
        TestStore.Stores.Any(x => x.Equals($"{nameof(TestRequestFilter)} {nameof(TestRequestFilter.OnExecuting)}")).ShouldBeTrue();
        TestStore.Stores.Clear();

        await mediator.PublishAsync(new TestEvent());
        TestStore.Stores.Count.ShouldBe(9);
        TestStore.Stores.Any(x => x.Equals($"{nameof(AllEventsFilter1)} {nameof(AllEventsFilter1.OnExecuting)}")).ShouldBeTrue();
        TestStore.Stores.Any(x => x.Equals($"{nameof(AllEventsFilter2)} {nameof(AllEventsFilter2.OnExecuting)}")).ShouldBeTrue();
        TestStore.Stores.Clear();
    }
    
    [Fact]
    public async Task ShouldMultipleFiltersInvokedExpectOrder()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseFilter<AllMessagesFilter2>()
            .UseFilter<AllCommandsFilter1>()
            .UseFilter<AllMessagesFilter1>()
            .UseFilter<AllCommandsFilter2>()
            .CreateMediator();

        var command = new TestCommand(Guid.NewGuid());
        
        await mediator.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(9);
        TestStore.Stores[0].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnExecuting)}");
        TestStore.Stores[3].ShouldBe($"{nameof(AllCommandsFilter2)} {nameof(AllCommandsFilter2.OnExecuting)}");
        TestStore.Stores[4].ShouldBe(command);
        TestStore.Stores[5].ShouldBe($"{nameof(AllCommandsFilter2)} {nameof(AllCommandsFilter2.OnExecuted)}");
        TestStore.Stores[6].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnExecuted)}");
        TestStore.Stores[7].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuted)}");
        TestStore.Stores[8].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnExecuted)}");
    }
    
    [Fact]
    public async Task ShouldCorrectResponseAcrossManyFilters()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestRequestHandler>()
            .RegisterHandler<TestEventHandler>()
            .RegisterHandler<TestCommandHasResponseHandler>()
            .UseFilter<AllMessagesFilter1>()
            .UseFilter<AllMessagesFilter2>()
            .UseFilter<AllCommandsFilter1>()
            .UseFilter<AllCommandsFilter2>()
            .UseFilter<AllRequestsFilter1>()
            .UseFilter<AllEventsFilter1>()
            .UseFilter<AllEventsFilter2>()
            .UseFilter(typeof(TestCommandFilter))
            .UseFilter(typeof(TestOtherCommandFilter))
            .UseFilter(typeof(TestRequestFilter))
            .CreateMediator();

        await mediator.PublishAsync(new TestEvent());
        
        var requestResponse = await mediator.RequestAsync<TestRequest, TestResponse>(new TestRequest());
        var commandResponse = await mediator.SendAsync<TestCommand, TestResponse>(new TestCommand(Guid.NewGuid()));
        
        requestResponse.Result.ShouldBe("Test response");
        commandResponse.Result.ShouldBe("Test command response");
    }
    
    [Fact]
    public void CannotUseNotAssignableFromIFilterInterface()
    {
        var func = () => new MediatorConfiguration()
            .UseFilter(typeof(TestCommandHandler));

        func.ShouldThrow<NotSupportedException>();
    }
}