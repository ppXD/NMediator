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
using NMediator.Test.TestData.Filters.ExceptionFilters;
using NMediator.Test.TestData.Filters.MessageFilters;
using NMediator.Test.TestData.Filters.RequestFilters;
using NMediator.Test.TestData.RequestHandlers;
using NMediator.Test.TestData.Requests;
using NMediator.Test.TestData.Responses;
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

        var command = new TestCommand();
        
        await mediator.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(3);
    }

    [Fact]
    public async Task ShouldExceptionFilterInvoked()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestExceptionCommandHandler>()
            .UseFilter<ExceptionUnHandledFilter1>()
            .UseFilter<ExceptionHandledFilter1>()
            .UseFilter<ExceptionHandledFilter2>()
            .CreateMediator();

        var command = new TestExceptionCommand();

        await mediator.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(3);
        TestStore.Stores[0].ShouldBe(command);
        TestStore.Stores[1].ShouldBe($"{nameof(ExceptionUnHandledFilter1)} {nameof(ExceptionUnHandledFilter1.OnException)}");
        TestStore.Stores[2].ShouldBe($"{nameof(ExceptionHandledFilter1)} {nameof(ExceptionHandledFilter1.OnException)}");
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
        
        var testCommand = new TestCommand();
        var otherCommand = new TestOtherCommand();
        
        await mediator.SendAsync(testCommand);
        TestStore.Stores.Count.ShouldBe(11);
        TestStore.Stores.Any(x => x.Equals($"{nameof(TestCommandFilter)} {nameof(TestCommandFilter.OnHandlerExecuting)}")).ShouldBeTrue();
        TestStore.Stores.Clear();
        
        await mediator.SendAsync(otherCommand);
        TestStore.Stores.Count.ShouldBe(11);
        TestStore.Stores.Any(x => x.Equals($"{nameof(TestOtherCommandFilter)} {nameof(TestOtherCommandFilter.OnHandlerExecuting)}")).ShouldBeTrue();
        TestStore.Stores.Clear();

        var response = await mediator.RequestAsync<TestResponse>(new TestRequest());
        response.ShouldNotBeNull();
        TestStore.Stores.Count.ShouldBe(9);
        TestStore.Stores.Any(x => x.Equals($"{nameof(TestRequestFilter)} {nameof(TestRequestFilter.OnHandlerExecuting)}")).ShouldBeTrue();
        TestStore.Stores.Clear();

        await mediator.PublishAsync(new TestEvent());
        TestStore.Stores.Count.ShouldBe(9);
        TestStore.Stores.Any(x => x.Equals($"{nameof(AllEventsFilter1)} {nameof(AllEventsFilter1.OnHandlerExecuting)}")).ShouldBeTrue();
        TestStore.Stores.Any(x => x.Equals($"{nameof(AllEventsFilter2)} {nameof(AllEventsFilter2.OnHandlerExecuting)}")).ShouldBeTrue();
        TestStore.Stores.Clear();
    }
    
    [Fact]
    public async Task ShouldMultipleMiddlewaresAndFiltersInvokedExpectOrder()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseFilter<AllMessagesFilter2>()
            .UseFilter<AllCommandsFilter1>()
            .UseFilter<AllMessagesFilter1>()
            .UseFilter<AllCommandsFilter2>()
            .CreateMediator();

        var command = new TestCommand();
        
        await mediator.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(9);
        TestStore.Stores[0].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnHandlerExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnHandlerExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnHandlerExecuting)}");
        TestStore.Stores[3].ShouldBe($"{nameof(AllCommandsFilter2)} {nameof(AllCommandsFilter2.OnHandlerExecuting)}");
        TestStore.Stores[4].ShouldBe(command);
        TestStore.Stores[5].ShouldBe($"{nameof(AllCommandsFilter2)} {nameof(AllCommandsFilter2.OnHandlerExecuted)}");
        TestStore.Stores[6].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnHandlerExecuted)}");
        TestStore.Stores[7].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnHandlerExecuted)}");
        TestStore.Stores[8].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnHandlerExecuted)}");
    }

    [Fact]
    public async Task ShouldHandlerThrowExceptionExpectOrder()
    {
        var mediator1 = new MediatorConfiguration()
            .RegisterHandler<TestExceptionCommandHandler>()
            .UseFilter<AllMessagesFilter2>()
            .UseFilter<AllCommandsFilter1>()
            .UseFilter<ExceptionHandledFilter1>()
            .CreateMediator();

        var command = new TestExceptionCommand();
        
        await mediator1.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(6);
        TestStore.Stores[0].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnHandlerExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnHandlerExecuting)}");
        TestStore.Stores[2].ShouldBe(command);
        TestStore.Stores[3].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnHandlerExecuted)}");
        TestStore.Stores[4].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnHandlerExecuted)}");
        TestStore.Stores[5].ShouldBe($"{nameof(ExceptionHandledFilter1)} {nameof(ExceptionHandledFilter1.OnException)}");
        TestStore.Stores.Clear();

        var mediator2 =  new MediatorConfiguration()
            .RegisterHandler<TestExceptionCommandHandler>()
            .UseFilter<AllMessagesFilter2>()
            .UseFilter<AllCommandsFilter1>()
            .UseFilter<ExceptionUnHandledFilter1>()
            .CreateMediator();

        try
        {
            await mediator2.SendAsync(command);
        }
        catch
        {
            // ignored
        }
        
        TestStore.Stores.Count.ShouldBe(6);
        TestStore.Stores[0].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnHandlerExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnHandlerExecuting)}");
        TestStore.Stores[2].ShouldBe(command);
        TestStore.Stores[3].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnHandlerExecuted)}");
        TestStore.Stores[4].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnHandlerExecuted)}");
        TestStore.Stores[5].ShouldBe($"{nameof(ExceptionUnHandledFilter1)} {nameof(ExceptionUnHandledFilter1.OnException)}");
    }

    [Fact]
    public async Task ShouldFilterThrowExceptionExpectOrder()
    {
        var command = new TestCommand();

        var mediator1 = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseFilter<AllMessagesFilter1>()
            .UseFilter<AllCommandsFilter1>()
            .UseFilter<TestCommandOnExecutingThrowExceptionFilter>()
            .UseFilter<ExceptionHandledFilter1>()
            .CreateMediator();
        
        await mediator1.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(6);
        TestStore.Stores[0].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnHandlerExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnHandlerExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(TestCommandOnExecutingThrowExceptionFilter)} {nameof(TestCommandOnExecutingThrowExceptionFilter.OnHandlerExecuting)}");
        TestStore.Stores[3].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnHandlerExecuted)}");
        TestStore.Stores[4].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnHandlerExecuted)}");
        TestStore.Stores[5].ShouldBe($"{nameof(ExceptionHandledFilter1)} {nameof(ExceptionHandledFilter1.OnException)}");
        TestStore.Stores.Clear();
        
        var mediator2 = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseFilter<AllMessagesFilter1>()
            .UseFilter<AllCommandsFilter1>()
            .UseFilter<TestCommandOnExecutedThrowExceptionFilter>()
            .UseFilter<ExceptionUnHandledFilter1>()
            .CreateMediator();
        
        try
        {
            await mediator2.SendAsync(command);
        }
        catch
        {
            // ignored
        }
        
        TestStore.Stores.Count.ShouldBe(8);
        TestStore.Stores[0].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnHandlerExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnHandlerExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(TestCommandOnExecutedThrowExceptionFilter)} {nameof(TestCommandOnExecutedThrowExceptionFilter.OnHandlerExecuting)}");
        TestStore.Stores[3].ShouldBe(command);
        TestStore.Stores[4].ShouldBe($"{nameof(TestCommandOnExecutedThrowExceptionFilter)} {nameof(TestCommandOnExecutedThrowExceptionFilter.OnHandlerExecuted)}");
        TestStore.Stores[5].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnHandlerExecuted)}");
        TestStore.Stores[6].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnHandlerExecuted)}");
        TestStore.Stores[7].ShouldBe($"{nameof(ExceptionUnHandledFilter1)} {nameof(ExceptionUnHandledFilter1.OnException)}");
    }
    
    [Fact]
    public async Task ShouldCorrectResponseAcrossManyFilters()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestRequestHandler>()
            .RegisterHandler<TestEventHandler>()
            .RegisterHandler<TestHasResponseCommandHandler>()
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
        
        var requestResponse = await mediator.RequestAsync<TestResponse>(new TestRequest());
        var commandResponse = await mediator.SendAsync(new TestHasResponseCommand());
        
        requestResponse.Result.ShouldBe("Test response");
        commandResponse.Result.ShouldBe("Test command response");
    }

    [Fact]
    public async Task ShouldInheritMessageFilterWork()
    {
        var cmdMediator1 = new MediatorConfiguration()
            .UseFilter<ITestCommandFilter>()
            .RegisterHandler<ITestCommandHandler>()
            .CreateMediator();
        
        var cmdMediator2 = new MediatorConfiguration()
            .UseFilter<ITestCommandFilter>()
            .UseFilter<TestCommandAllWayCommandFilter>()
            .RegisterHandler<ITestCommandHandler>()
            .CreateMediator();
        
        var reqMediator1 = new MediatorConfiguration()
            .UseFilter<ITestRequestFilter>()
            .RegisterHandler<ITestRequestHandler>()
            .CreateMediator();
        
        var reqMediator2 = new MediatorConfiguration()
            .UseFilter<ITestRequestFilter>()
            .UseFilter<TestAllWayRequestFilter>()
            .RegisterHandler<ITestRequestHandler>()
            .CreateMediator();
        
        await cmdMediator1.SendAsync(new TestCommandOneWayCommand());
        await reqMediator1.RequestAsync<TestResponse>(new TestInterfaceRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestCommandFilter)} {nameof(ITestCommandFilter.OnHandlerExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestCommandHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(ITestCommandFilter)} {nameof(ITestCommandFilter.OnHandlerExecuted)}");
        
        TestStore.Stores[3].ShouldBe($"{nameof(ITestRequestFilter)} {nameof(ITestRequestFilter.OnHandlerExecuting)}");
        TestStore.Stores[4].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[5].ShouldBe($"{nameof(ITestRequestFilter)} {nameof(ITestRequestFilter.OnHandlerExecuted)}");
        TestStore.Stores.Clear();

        await cmdMediator2.SendAsync(new TestInheritAllWayCommand());
        await reqMediator2.RequestAsync<TestResponse>(new TestInheritAllWayRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestCommandFilter)} {nameof(ITestCommandFilter.OnHandlerExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestCommandAllWayCommandFilter)} {nameof(TestCommandAllWayCommandFilter.OnHandlerExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(ITestCommandHandler)}");
        TestStore.Stores[3].ShouldBe($"{nameof(TestCommandAllWayCommandFilter)} {nameof(TestCommandAllWayCommandFilter.OnHandlerExecuted)}");
        TestStore.Stores[4].ShouldBe($"{nameof(ITestCommandFilter)} {nameof(ITestCommandFilter.OnHandlerExecuted)}");
        
        TestStore.Stores[5].ShouldBe($"{nameof(ITestRequestFilter)} {nameof(ITestRequestFilter.OnHandlerExecuting)}");
        TestStore.Stores[6].ShouldBe($"{nameof(TestAllWayRequestFilter)} {nameof(TestAllWayRequestFilter.OnHandlerExecuting)}");
        TestStore.Stores[7].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[8].ShouldBe($"{nameof(TestAllWayRequestFilter)} {nameof(TestAllWayRequestFilter.OnHandlerExecuted)}");
        TestStore.Stores[9].ShouldBe($"{nameof(ITestRequestFilter)} {nameof(ITestRequestFilter.OnHandlerExecuted)}");
        TestStore.Stores.Clear();

        await cmdMediator2.SendAsync(new TestParentInheritCommand());
        await reqMediator2.RequestAsync<TestResponse>(new TestParentInheritRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestCommandFilter)} {nameof(ITestCommandFilter.OnHandlerExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestCommandAllWayCommandFilter)} {nameof(TestCommandAllWayCommandFilter.OnHandlerExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(ITestCommandHandler)}");
        TestStore.Stores[3].ShouldBe($"{nameof(TestCommandAllWayCommandFilter)} {nameof(TestCommandAllWayCommandFilter.OnHandlerExecuted)}");
        TestStore.Stores[4].ShouldBe($"{nameof(ITestCommandFilter)} {nameof(ITestCommandFilter.OnHandlerExecuted)}");
        
        TestStore.Stores[5].ShouldBe($"{nameof(ITestRequestFilter)} {nameof(ITestRequestFilter.OnHandlerExecuting)}");
        TestStore.Stores[6].ShouldBe($"{nameof(TestAllWayRequestFilter)} {nameof(TestAllWayRequestFilter.OnHandlerExecuting)}");
        TestStore.Stores[7].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[8].ShouldBe($"{nameof(TestAllWayRequestFilter)} {nameof(TestAllWayRequestFilter.OnHandlerExecuted)}");
        TestStore.Stores[9].ShouldBe($"{nameof(ITestRequestFilter)} {nameof(ITestRequestFilter.OnHandlerExecuted)}");
        TestStore.Stores.Clear();
        
        await cmdMediator2.SendAsync(new TestCommandOneWayCommand());
        await reqMediator2.RequestAsync<TestResponse>(new TestOneWayRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestCommandFilter)} {nameof(ITestCommandFilter.OnHandlerExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestCommandHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(ITestCommandFilter)} {nameof(ITestCommandFilter.OnHandlerExecuted)}");
        
        TestStore.Stores[3].ShouldBe($"{nameof(ITestRequestFilter)} {nameof(ITestRequestFilter.OnHandlerExecuting)}");
        TestStore.Stores[4].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[5].ShouldBe($"{nameof(ITestRequestFilter)} {nameof(ITestRequestFilter.OnHandlerExecuted)}");
        TestStore.Stores.Clear();
        
        await cmdMediator2.SendAsync(new TestCommandTwoWayCommand());
        await reqMediator2.RequestAsync<TestResponse>(new TestTwoWayRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestCommandFilter)} {nameof(ITestCommandFilter.OnHandlerExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestCommandHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(ITestCommandFilter)} {nameof(ITestCommandFilter.OnHandlerExecuted)}");
        
        TestStore.Stores[3].ShouldBe($"{nameof(ITestRequestFilter)} {nameof(ITestRequestFilter.OnHandlerExecuting)}");
        TestStore.Stores[4].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[5].ShouldBe($"{nameof(ITestRequestFilter)} {nameof(ITestRequestFilter.OnHandlerExecuted)}");
    }
    
    [Fact]
    public void CannotUseNotAssignableFromIFilterInterface()
    {
        var configureFunc = () => new MediatorConfiguration()
            .UseFilter(typeof(TestCommandHandler));

        configureFunc.ShouldThrow<Exception>();
    }
}