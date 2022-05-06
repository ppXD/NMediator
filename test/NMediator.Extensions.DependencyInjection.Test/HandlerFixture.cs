using System.Linq;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Handlers.CommandHandlers;
using NMediator.Extensions.DependencyInjection.Test.Handlers.EventHandlers;
using NMediator.Extensions.DependencyInjection.Test.Handlers.RequestHandlers;
using NMediator.Extensions.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.DependencyInjection.Test.Messages.Events;
using NMediator.Extensions.DependencyInjection.Test.Messages.Requests;
using Shouldly;
using Xunit;

namespace NMediator.Extensions.DependencyInjection.Test;

public class HandlerFixture : TestFixtureBase
{
    [Theory]
    [InlineData(DependencyInjectionType.Autofac, 1)]
    [InlineData(DependencyInjectionType.Autofac, 2)]
    [InlineData(DependencyInjectionType.Autofac, 3)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection, 1)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection, 2)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection, 3)]
    public async Task ShouldHandlerResolved(DependencyInjectionType dependencyInjectionType, int way)
    {
        RegisterMediator(dependencyInjectionType, config =>
        {
            config.RegisterHandler<TestCommandHandler>();
            config.RegisterHandler<TestRequestHandler>();
            config.RegisterHandler<TestEventHandler>();
        }, new[] { typeof(HandlerFixture).Assembly }, way);

        var mediator = GetMediator<IMediator>(dependencyInjectionType);

        await mediator.SendAsync(new TestCommand());
        
        Logger.Messages.Count.ShouldBe(1);
        Logger.Messages.Single().ShouldBe(nameof(TestCommand));
        Logger.Messages.Clear();

        var response = await mediator.RequestAsync(new TestRequest());

        response.ShouldNotBeNull();
        Logger.Messages.Count.ShouldBe(1);
        Logger.Messages.Single().ShouldBe(nameof(TestRequest));
        Logger.Messages.Clear();

        await mediator.PublishAsync(new TestEvent());
        
        Logger.Messages.Count.ShouldBe(1);
        Logger.Messages.Single().ShouldBe(nameof(TestEvent));
    }

    [Theory]
    [InlineData(DependencyInjectionType.Autofac)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection)]
    public async Task ShouldResolvedRegisterMultipleHandler(DependencyInjectionType dependencyInjectionType)
    {
        RegisterMediator(dependencyInjectionType, config =>
        {
            config.RegisterHandler<TestCommandHandler>();
        }, new[] { typeof(HandlerFixture).Assembly });

        var mediator = GetMediator<IMediator>(dependencyInjectionType);

        await mediator.SendAsync(new TestCommand());
        
        Logger.Messages.Count.ShouldBe(1);
        Logger.Messages.Single().ShouldBe(nameof(TestCommand));
    }

    [Theory]
    [InlineData(DependencyInjectionType.Autofac)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection)]
    public async Task ShouldHandlerReturnResponse(DependencyInjectionType dependencyInjectionType)
    {
        RegisterMediator(dependencyInjectionType, config =>
        {
        }, new[] { typeof(HandlerFixture).Assembly });

        var mediator = GetMediator<IMediator>(dependencyInjectionType);

        var response = await mediator.SendAsync(new TestCommand());

        response.ShouldNotBeNull();
    }

    [Theory]
    [InlineData(DependencyInjectionType.Autofac)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection)]
    public async Task ShouldHandlerPublishEvent(DependencyInjectionType dependencyInjectionType)
    {
        RegisterMediator(dependencyInjectionType, config =>
        {
        }, new[] { typeof(HandlerFixture).Assembly });

        var mediator = GetMediator<IMediator>(dependencyInjectionType);

        await mediator.SendAsync(new PublishEventCommand());
        
        Logger.Messages.Count.ShouldBe(2);
        Logger.Messages.ShouldBe(new []
        {
            $"{nameof(PublishEventCommand)}",
            $"{nameof(TestEvent)}",
        });
    }
    
    [Theory]
    [InlineData(DependencyInjectionType.Autofac)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection)]
    public async Task ShouldDuplicatedAssembliesWork(DependencyInjectionType dependencyInjectionType)
    {
        RegisterMediator(dependencyInjectionType, config =>
        {
        }, new[] { typeof(HandlerFixture).Assembly, typeof(HandlerFixture).Assembly });

        var mediator = GetMediator<IMediator>(dependencyInjectionType);

        var response = await mediator.RequestAsync(new TestRequest());

        response.ShouldNotBeNull();
        Logger.Messages.Count.ShouldBe(1);
        Logger.Messages.Single().ShouldBe(nameof(TestRequest));
    }
}