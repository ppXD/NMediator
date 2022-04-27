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
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuting)}",
            $"{nameof(TestCommandMessageFilter)} {nameof(TestCommandMessageFilter.OnExecuting)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnExecuting)}",
            $"{nameof(TestCommandFilter)} {nameof(TestCommandFilter.OnExecuting)}",
            $"{nameof(TestCommand)}",
            $"{nameof(TestCommandFilter)} {nameof(TestCommandFilter.OnExecuted)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnExecuted)}",
            $"{nameof(TestCommandMessageFilter)} {nameof(TestCommandMessageFilter.OnExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuted)}"
        });
        Logger.Messages.Clear();
        
        var response = await mediator.RequestAsync(new TestRequest());

        response.ShouldNotBeNull();
        Logger.Messages.Count.ShouldBe(7);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuting)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnExecuting)}",
            $"{nameof(TestRequestFilter)} {nameof(TestRequestFilter.OnExecuting)}",
            $"{nameof(TestRequest)}",
            $"{nameof(TestRequestFilter)} {nameof(TestRequestFilter.OnExecuted)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuted)}"
        });
        Logger.Messages.Clear();
        
        await mediator.PublishAsync(new TestEvent());
        
        Logger.Messages.Count.ShouldBe(7);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuting)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnExecuting)}",
            $"{nameof(TestEventFilter)} {nameof(TestEventFilter.OnExecuting)}",
            $"{nameof(TestEvent)}",
            $"{nameof(TestEventFilter)} {nameof(TestEventFilter.OnExecuted)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuted)}"
        });
        Logger.Messages.Clear();
        
        await mediator.SendAsync(new ThrowExceptionCommand());
        
        Logger.Messages.Count.ShouldBe(6);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuting)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnExecuting)}",
            $"{nameof(ThrowExceptionCommand)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuted)}",
            $"{nameof(ExceptionFilter)} {nameof(ExceptionFilter.OnException)}"
        });
    }
}