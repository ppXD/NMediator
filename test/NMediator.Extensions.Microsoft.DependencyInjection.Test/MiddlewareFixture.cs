using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Events;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Requests;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Middlewares;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;
using Shouldly;
using Xunit;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test;

public class MiddlewareFixture
{
    private readonly Logger _logger;
    private readonly IServiceCollection _services;
    
    public MiddlewareFixture()
    {
        _logger = new Logger();
        _services = new ServiceCollection();
        
        _services.AddSingleton(_logger);
        _services.AddScoped<ILogService, LogService>();
        _services.AddScoped<IDoNothingService, DoNothingService>();
    }
    
    [Fact]
    public async Task ShouldMiddlewareResolved()
    {
        _services.AddNMediator(config =>
        {
            config.UseMiddleware<TestMiddleware1>();
            config.UseMiddleware(typeof(TestMiddleware2));
        }, typeof(MiddlewareFixture).Assembly);
                
        var mediator = _services.BuildServiceProvider().GetRequiredService<IMediator>();

        await mediator.SendAsync(new TestCommand());
        
        _logger.Messages.Count.ShouldBe(5);
        _logger.Messages.ShouldBe(new []
        {
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuting)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuting)}",
            $"{nameof(TestCommand)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuted)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuted)}"
        });
        _logger.Messages.Clear();

        var response = await mediator.RequestAsync<TestRequest, TestResponse>(new TestRequest());

        response.ShouldNotBeNull();
        _logger.Messages.Count.ShouldBe(5);
        _logger.Messages.ShouldBe(new []
        {
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuting)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuting)}",
            $"{nameof(TestRequest)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuted)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuted)}"
        });
        _logger.Messages.Clear();

        await mediator.PublishAsync(new TestEvent());
        
        _logger.Messages.Count.ShouldBe(5);
        _logger.Messages.ShouldBe(new []
        {
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuting)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuting)}",
            $"{nameof(TestEvent)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuted)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuted)}"
        });
    }

    [Fact]
    public async Task ShouldRegisterSameMiddlewareMultiple()
    {
        _services.AddNMediator(config =>
        {
            config
                .UseMiddleware<TestMiddleware1>()
                .UseMiddleware<TestMiddleware1>();
        }, typeof(MiddlewareFixture).Assembly);
                
        var mediator = _services.BuildServiceProvider().GetRequiredService<IMediator>();

        await mediator.SendAsync(new TestCommand());
        
        _logger.Messages.Count.ShouldBe(5);
        _logger.Messages.ShouldBe(new []
        {
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuting)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuting)}",
            $"{nameof(TestCommand)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuted)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuted)}"
        });
    }
}