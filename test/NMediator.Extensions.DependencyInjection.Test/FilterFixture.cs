using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Filters.CommandFilters;
using NMediator.Extensions.DependencyInjection.Test.Filters.EventFilters;
using NMediator.Extensions.DependencyInjection.Test.Filters.ExceptionFilters;
using NMediator.Extensions.DependencyInjection.Test.Filters.MessageFilters;
using NMediator.Extensions.DependencyInjection.Test.Filters.RequestFilters;
using NMediator.Extensions.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.DependencyInjection.Test.Messages.Events;
using NMediator.Extensions.DependencyInjection.Test.Messages.Requests;
using Shouldly;
using Xunit;

namespace NMediator.Extensions.DependencyInjection.Test;

public class FilterFixture : TestFixtureBase
{
    [Theory]
    [InlineData(DependencyInjectionType.Autofac)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection)]
    public async Task ShouldFilterResolved(DependencyInjectionType dependencyInjectionType)
    {
        RegisterMediator(dependencyInjectionType, config =>
        {
            config
                .UseFilter<AllMessagesFilter>()
                .UseFilter<TestCommandMessageFilter>()
                .UseFilter<AllCommandsFilter>()
                .UseFilter<TestCommandFilter>()
                .UseFilter<AllRequestsFilter>()
                .UseFilter<TestRequestFilter>()
                .UseFilter<AllEventsFilter>()
                .UseFilter<TestEventFilter>()
                .UseFilter<ExceptionFilter>();
        }, new[] { typeof(FilterFixture).Assembly });
                
        var mediator = GetMediator<IMediator>(dependencyInjectionType);

        await mediator.SendAsync(new TestCommand());
        
        Logger.Messages.Count.ShouldBe(9);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuting)}",
            $"{nameof(TestCommandMessageFilter)} {nameof(TestCommandMessageFilter.OnHandlerExecuting)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnHandlerExecuting)}",
            $"{nameof(TestCommandFilter)} {nameof(TestCommandFilter.OnHandlerExecuting)}",
            $"{nameof(TestCommand)}",
            $"{nameof(TestCommandFilter)} {nameof(TestCommandFilter.OnHandlerExecuted)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnHandlerExecuted)}",
            $"{nameof(TestCommandMessageFilter)} {nameof(TestCommandMessageFilter.OnHandlerExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuted)}"
        });
        Logger.Messages.Clear();
        
        var response = await mediator.RequestAsync(new TestRequest());

        response.ShouldNotBeNull();
        Logger.Messages.Count.ShouldBe(7);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuting)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnHandlerExecuting)}",
            $"{nameof(TestRequestFilter)} {nameof(TestRequestFilter.OnHandlerExecuting)}",
            $"{nameof(TestRequest)}",
            $"{nameof(TestRequestFilter)} {nameof(TestRequestFilter.OnHandlerExecuted)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnHandlerExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuted)}"
        });
        Logger.Messages.Clear();
        
        await mediator.PublishAsync(new TestEvent());
        
        Logger.Messages.Count.ShouldBe(7);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuting)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnHandlerExecuting)}",
            $"{nameof(TestEventFilter)} {nameof(TestEventFilter.OnHandlerExecuting)}",
            $"{nameof(TestEvent)}",
            $"{nameof(TestEventFilter)} {nameof(TestEventFilter.OnHandlerExecuted)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnHandlerExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuted)}"
        });
        Logger.Messages.Clear();
        
        await mediator.SendAsync(new ThrowExceptionCommand());
        
        Logger.Messages.Count.ShouldBe(6);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuting)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnHandlerExecuting)}",
            $"{nameof(ThrowExceptionCommand)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnHandlerExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuted)}",
            $"{nameof(ExceptionFilter)} {nameof(ExceptionFilter.OnException)}"
        });
    }
}