using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.DependencyInjection.Test.Messages.Events;
using NMediator.Extensions.DependencyInjection.Test.Messages.Requests;
using NMediator.Extensions.DependencyInjection.Test.Middlewares;
using Shouldly;
using Xunit;

namespace NMediator.Extensions.DependencyInjection.Test;

public class MiddlewareFixture : TestFixtureBase
{
    [Theory]
    [InlineData(DependencyInjectionType.Autofac)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection)]
    public async Task ShouldMiddlewareResolved(DependencyInjectionType dependencyInjectionType)
    {
        RegisterMediator(dependencyInjectionType, config =>
        {
            config.UseMiddleware<TestMiddleware1>();
            config.UseMiddleware(typeof(TestMiddleware2));
        }, new[] { typeof(MiddlewareFixture).Assembly });
                
        var mediator = GetMediator<IMediator>(dependencyInjectionType);

        await mediator.SendAsync(new TestCommand());
        
        Logger.Messages.Count.ShouldBe(5);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuting)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuting)}",
            $"{nameof(TestCommand)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuted)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuted)}"
        });
        Logger.Messages.Clear();

        var response = await mediator.RequestAsync(new TestRequest());

        response.ShouldNotBeNull();
        Logger.Messages.Count.ShouldBe(5);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuting)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuting)}",
            $"{nameof(TestRequest)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuted)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuted)}"
        });
        Logger.Messages.Clear();

        await mediator.PublishAsync(new TestEvent());
        
        Logger.Messages.Count.ShouldBe(5);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuting)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuting)}",
            $"{nameof(TestEvent)}",
            $"{nameof(TestMiddleware2)} {nameof(TestMiddleware2.OnExecuted)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuted)}"
        });
    }

    [Theory]
    [InlineData(DependencyInjectionType.Autofac)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection)]
    public async Task ShouldRegisterSameMiddlewareMultiple(DependencyInjectionType dependencyInjectionType)
    {
        RegisterMediator(dependencyInjectionType, config =>
        {
            config
                .UseMiddleware<TestMiddleware1>()
                .UseMiddleware<TestMiddleware1>();
        }, new[] { typeof(MiddlewareFixture).Assembly });
                
        var mediator = GetMediator<IMediator>(dependencyInjectionType);

        await mediator.SendAsync(new TestCommand());
        
        Logger.Messages.Count.ShouldBe(5);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuting)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuting)}",
            $"{nameof(TestCommand)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuted)}",
            $"{nameof(TestMiddleware1)} {nameof(TestMiddleware1.OnExecuted)}"
        });
    }
}