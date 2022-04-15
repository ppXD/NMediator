using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Handlers.CommandHandlers;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;
using Shouldly;
using Xunit;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test;

public class HandlerFixture
{
    private readonly Logger _logger;
    
    public HandlerFixture()
    {
        _logger = new Logger();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task ShouldHandlerResolved(int way)
    {
        var services = new ServiceCollection();
        
        services.AddSingleton(_logger);
        services.AddScoped<ILogService, LogService>();
        
        switch (way)
        {
            case 1:
                services.AddNMediator(typeof(HandlerFixture).Assembly);
                break;
            case 2:
                services.AddNMediator(config =>
                {
                    config.RegisterHandler<TestCommandHandler>();
                });
                break;
            case 3:
                services.AddNMediator(config =>
                {

                }, typeof(HandlerFixture).Assembly);
                break;
        }

        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        await mediator.SendAsync(new TestCommand());
        
        _logger.Messages.Count.ShouldBe(1);
        _logger.Messages.Single().ShouldBe(nameof(TestCommand));
    }

    [Fact]
    public async Task ShouldResolvedRegisterMultipleHandler()
    {
        var services = new ServiceCollection();
        
        services.AddSingleton(_logger);
        services.AddScoped<ILogService, LogService>();

        services.AddNMediator(config =>
        {
            config.RegisterHandler<TestCommandHandler>();
        }, typeof(HandlerFixture).Assembly);

        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        await mediator.SendAsync(new TestCommand());
        
        _logger.Messages.Count.ShouldBe(1);
        _logger.Messages.Single().ShouldBe(nameof(TestCommand));
    }
}