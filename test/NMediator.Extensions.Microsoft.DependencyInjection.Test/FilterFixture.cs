using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Filters.CommandFilters;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Filters.EventFilters;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Filters.ExceptionFilters;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Filters.MessageFilters;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Filters.RequestFilters;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Events;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Requests;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;
using Shouldly;
using Xunit;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test;

public class FilterFixture
{
    private readonly Logger _logger;
    private readonly IServiceCollection _services;
    
    public FilterFixture()
    {
        _logger = new Logger();
        _services = new ServiceCollection();
        
        _services.AddSingleton(_logger);
        _services.AddScoped<ILogService, LogService>();
        _services.AddScoped<IDoNothingService, DoNothingService>();
    }

    [Fact]
    public async Task ShouldFilterResolved()
    {
        _services.AddNMediator(config =>
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
            
        }, typeof(MiddlewareFixture).Assembly);
                
        var mediator = _services.BuildServiceProvider().GetRequiredService<IMediator>();

        await mediator.SendAsync(new TestCommand());
        
        _logger.Messages.Count.ShouldBe(9);
        _logger.Messages.ShouldBe(new []
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
        _logger.Messages.Clear();
        
        var response = await mediator.RequestAsync<TestRequest, TestResponse>(new TestRequest());

        response.ShouldNotBeNull();
        _logger.Messages.Count.ShouldBe(7);
        _logger.Messages.ShouldBe(new []
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuting)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnExecuting)}",
            $"{nameof(TestRequestFilter)} {nameof(TestRequestFilter.OnExecuting)}",
            $"{nameof(TestRequest)}",
            $"{nameof(TestRequestFilter)} {nameof(TestRequestFilter.OnExecuted)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuted)}"
        });
        _logger.Messages.Clear();
        
        await mediator.PublishAsync(new TestEvent());
        
        _logger.Messages.Count.ShouldBe(7);
        _logger.Messages.ShouldBe(new []
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuting)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnExecuting)}",
            $"{nameof(TestEventFilter)} {nameof(TestEventFilter.OnExecuting)}",
            $"{nameof(TestEvent)}",
            $"{nameof(TestEventFilter)} {nameof(TestEventFilter.OnExecuted)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuted)}"
        });
    }
}