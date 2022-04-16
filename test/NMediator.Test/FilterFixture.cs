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
using NMediator.Test.TestData.Middlewares;
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
    public async Task ShouldMultipleMiddlewaresAndFiltersInvokedExpectOrder()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseMiddleware<TestFirstMiddleware>()
            .UseFilter<AllMessagesFilter2>()
            .UseMiddleware<TestSecondMiddleware>()
            .UseFilter<AllCommandsFilter1>()
            .UseFilter<AllMessagesFilter1>()
            .UseFilter<AllCommandsFilter2>()
            .UseMiddleware<TestThirdMiddleware>()
            .CreateMediator();

        var command = new TestCommand(Guid.NewGuid());
        
        await mediator.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(15);
        TestStore.Stores[0].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestSecondMiddleware)} {nameof(TestSecondMiddleware.OnExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(TestThirdMiddleware)} {nameof(TestThirdMiddleware.OnExecuting)}");
        TestStore.Stores[3].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnExecuting)}");
        TestStore.Stores[4].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuting)}");
        TestStore.Stores[5].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnExecuting)}");
        TestStore.Stores[6].ShouldBe($"{nameof(AllCommandsFilter2)} {nameof(AllCommandsFilter2.OnExecuting)}");
        TestStore.Stores[7].ShouldBe(command);
        TestStore.Stores[8].ShouldBe($"{nameof(AllCommandsFilter2)} {nameof(AllCommandsFilter2.OnExecuted)}");
        TestStore.Stores[9].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnExecuted)}");
        TestStore.Stores[10].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuted)}");
        TestStore.Stores[11].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnExecuted)}");
        TestStore.Stores[12].ShouldBe($"{nameof(TestThirdMiddleware)} {nameof(TestThirdMiddleware.OnExecuted)}");
        TestStore.Stores[13].ShouldBe($"{nameof(TestSecondMiddleware)} {nameof(TestSecondMiddleware.OnExecuted)}");
        TestStore.Stores[14].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuted)}");
    }

    [Fact]
    public async Task ShouldHandlerThrowExceptionExpectOrder()
    {
        var mediator1 = new MediatorConfiguration()
            .RegisterHandler<TestExceptionCommandHandler>()
            .UseMiddleware<TestFirstMiddleware>()
            .UseFilter<AllMessagesFilter2>()
            .UseFilter<AllCommandsFilter1>()
            .UseMiddleware<TestThirdMiddleware>()
            .UseFilter<ExceptionHandledFilter1>()
            .CreateMediator();

        var command = new TestExceptionCommand();
        
        await mediator1.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(10);
        TestStore.Stores[0].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestThirdMiddleware)} {nameof(TestThirdMiddleware.OnExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnExecuting)}");
        TestStore.Stores[3].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuting)}");
        TestStore.Stores[4].ShouldBe(command);
        TestStore.Stores[5].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuted)}");
        TestStore.Stores[6].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnExecuted)}");
        TestStore.Stores[7].ShouldBe($"{nameof(ExceptionHandledFilter1)} {nameof(ExceptionHandledFilter1.OnException)}");
        TestStore.Stores[8].ShouldBe($"{nameof(TestThirdMiddleware)} {nameof(TestThirdMiddleware.OnExecuted)}");
        TestStore.Stores[9].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuted)}");
        TestStore.Stores.Clear();

        var mediator2 =  new MediatorConfiguration()
            .RegisterHandler<TestExceptionCommandHandler>()
            .UseMiddleware<TestFirstMiddleware>()
            .UseFilter<AllMessagesFilter2>()
            .UseFilter<AllCommandsFilter1>()
            .UseMiddleware<TestThirdMiddleware>()
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
        TestStore.Stores[0].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestThirdMiddleware)} {nameof(TestThirdMiddleware.OnExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnExecuting)}");
        TestStore.Stores[3].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuting)}");
        TestStore.Stores[4].ShouldBe(command);
        TestStore.Stores[5].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuted)}");
        TestStore.Stores[6].ShouldBe($"{nameof(AllMessagesFilter2)} {nameof(AllMessagesFilter2.OnExecuted)}");
        TestStore.Stores[7].ShouldBe($"{nameof(ExceptionUnHandledFilter1)} {nameof(ExceptionUnHandledFilter1.OnException)}");
    }

    [Fact]
    public async Task ShouldFilterThrowExceptionExpectOrder()
    {
        var command = new TestCommand(Guid.NewGuid());

        var mediator1 = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseMiddleware<TestFirstMiddleware>()
            .UseFilter<AllMessagesFilter1>()
            .UseFilter<AllCommandsFilter1>()
            .UseFilter<TestCommandOnExecutingThrowExceptionFilter>()
            .UseFilter<ExceptionHandledFilter1>()
            .CreateMediator();
        
        await mediator1.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(8);
        TestStore.Stores[0].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuting)}");
        TestStore.Stores[3].ShouldBe($"{nameof(TestCommandOnExecutingThrowExceptionFilter)} {nameof(TestCommandOnExecutingThrowExceptionFilter.OnExecuting)}");
        TestStore.Stores[4].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuted)}");
        TestStore.Stores[5].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnExecuted)}");
        TestStore.Stores[6].ShouldBe($"{nameof(ExceptionHandledFilter1)} {nameof(ExceptionHandledFilter1.OnException)}");
        TestStore.Stores[7].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuted)}");
        TestStore.Stores.Clear();
        
        var mediator2 = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseMiddleware<TestFirstMiddleware>()
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
        
        TestStore.Stores.Count.ShouldBe(9);
        TestStore.Stores[0].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuting)}");
        TestStore.Stores[3].ShouldBe($"{nameof(TestCommandOnExecutedThrowExceptionFilter)} {nameof(TestCommandOnExecutedThrowExceptionFilter.OnExecuting)}");
        TestStore.Stores[4].ShouldBe(command);
        TestStore.Stores[5].ShouldBe($"{nameof(TestCommandOnExecutedThrowExceptionFilter)} {nameof(TestCommandOnExecutedThrowExceptionFilter.OnExecuted)}");
        TestStore.Stores[6].ShouldBe($"{nameof(AllCommandsFilter1)} {nameof(AllCommandsFilter1.OnExecuted)}");
        TestStore.Stores[7].ShouldBe($"{nameof(AllMessagesFilter1)} {nameof(AllMessagesFilter1.OnExecuted)}");
        TestStore.Stores[8].ShouldBe($"{nameof(ExceptionUnHandledFilter1)} {nameof(ExceptionUnHandledFilter1.OnException)}");
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
        var config = new MediatorConfiguration()
            .UseFilter(typeof(TestCommandHandler));

        config.Filters.Count.ShouldBe(0);
    }
}