using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Handlers.CommandHandlers;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Handlers.EventHandlers;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Handlers.RequestHandlers;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Events;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Requests;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;
using Shouldly;
using Xunit;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test;

public class HandlerFixture
{
    private readonly Logger _logger;
    private readonly IServiceCollection _services;
    
    public HandlerFixture()
    {
        _logger = new Logger();
        _services = new ServiceCollection();
        
        _services.AddSingleton(_logger);
        _services.AddScoped<ILogService, LogService>();
        _services.AddScoped<IDoNothingService, DoNothingService>();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task ShouldHandlerResolved(int way)
    {
        switch (way)
        {
            case 1:
                _services.AddNMediator(typeof(HandlerFixture).Assembly);
                break;
            case 2:
                _services.AddNMediator(config =>
                {
                    config.RegisterHandler<TestCommandHandler>();
                    config.RegisterHandler<TestRequestHandler>();
                    config.RegisterHandler<TestEventHandler>();
                });
                break;
            case 3:
                _services.AddNMediator(config =>
                {

                }, typeof(HandlerFixture).Assembly);
                break;
        }

        var mediator = _services.BuildServiceProvider().GetRequiredService<IMediator>();

        await mediator.SendAsync(new TestCommand());
        
        _logger.Messages.Count.ShouldBe(1);
        _logger.Messages.Single().ShouldBe(nameof(TestCommand));
        _logger.Messages.Clear();

        var response = await mediator.RequestAsync<TestRequest, TestResponse>(new TestRequest());

        response.ShouldNotBeNull();
        _logger.Messages.Count.ShouldBe(1);
        _logger.Messages.Single().ShouldBe(nameof(TestRequest));
        _logger.Messages.Clear();

        await mediator.PublishAsync(new TestEvent());
        
        _logger.Messages.Count.ShouldBe(1);
        _logger.Messages.Single().ShouldBe(nameof(TestEvent));
    }

    [Fact]
    public async Task ShouldResolvedRegisterMultipleHandler()
    {
        _services.AddNMediator(config =>
        {
            config.RegisterHandler<TestCommandHandler>();
        }, typeof(HandlerFixture).Assembly);

        var mediator = _services.BuildServiceProvider().GetRequiredService<IMediator>();

        await mediator.SendAsync(new TestCommand());
        
        _logger.Messages.Count.ShouldBe(1);
        _logger.Messages.Single().ShouldBe(nameof(TestCommand));
    }

    [Fact]
    public async Task ShouldHandlerReturnResponse()
    {
        _services.AddNMediator(typeof(HandlerFixture).Assembly);

        var mediator = _services.BuildServiceProvider().GetRequiredService<IMediator>();

        var response = await mediator.SendAsync<TestCommand, TestCommandResponse>(new TestCommand());

        response.ShouldNotBeNull();
    }

    [Fact]
    public async Task ShouldDuplicatedAssembliesWork()
    {
        _services.AddNMediator(typeof(HandlerFixture).Assembly, typeof(HandlerFixture).Assembly);

        var mediator = _services.BuildServiceProvider().GetRequiredService<IMediator>();

        var response = await mediator.RequestAsync<TestRequest, TestResponse>(new TestRequest());

        response.ShouldNotBeNull();
        _logger.Messages.Count.ShouldBe(1);
        _logger.Messages.Single().ShouldBe(nameof(TestRequest));
    }
}