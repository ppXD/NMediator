using System;
using System.Threading.Tasks;
using NMediator.Test.TestData;
using NMediator.Test.TestData.CommandHandlers;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.EventHandlers;
using NMediator.Test.TestData.Events;
using NMediator.Test.TestData.Middlewares;
using NMediator.Test.TestData.RequestHandlers;
using NMediator.Test.TestData.Requests;
using Shouldly;
using Xunit;

namespace NMediator.Test;

public class MiddlewareFixture : TestBase
{
    [Fact]
    public async Task ShouldMiddlewareInvoked()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseMiddleware<TestFirstMiddleware>()
            .CreateMediator();

        var command = new TestCommand(Guid.NewGuid());
        
        await mediator.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(3);
        TestStore.Stores[0].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuting)}");
        TestStore.Stores[1].ShouldBe(command);
        TestStore.Stores[2].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuted)}");
    }
    
    [Fact]
    public async Task ShouldMultipleMiddlewaresExecutedExpectOrder()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseMiddleware<TestFirstMiddleware>()
            .UseMiddleware<TestSecondMiddleware>()
            .UseMiddleware<TestThirdMiddleware>()
            .CreateMediator();

        var command = new TestCommand(Guid.NewGuid());
        
        await mediator.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(7);
        TestStore.Stores[0].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuting)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestSecondMiddleware)} {nameof(TestSecondMiddleware.OnExecuting)}");
        TestStore.Stores[2].ShouldBe($"{nameof(TestThirdMiddleware)} {nameof(TestThirdMiddleware.OnExecuting)}");
        TestStore.Stores[3].ShouldBe(command);
        TestStore.Stores[4].ShouldBe($"{nameof(TestThirdMiddleware)} {nameof(TestThirdMiddleware.OnExecuted)}");
        TestStore.Stores[5].ShouldBe($"{nameof(TestSecondMiddleware)} {nameof(TestSecondMiddleware.OnExecuted)}");
        TestStore.Stores[6].ShouldBe($"{nameof(TestFirstMiddleware)} {nameof(TestFirstMiddleware.OnExecuted)}");
    }

    [Fact]
    public async Task ShouldCorrectResponseAcrossManyMiddlewares()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestRequestHandler>()
            .RegisterHandler<TestEventHandler>()
            .RegisterHandler<TestCommandHasResponseHandler>()
            .UseMiddleware<TestFirstMiddleware>()
            .UseMiddleware<TestSecondMiddleware>()
            .UseMiddleware<TestThirdMiddleware>()
            .CreateMediator();

        await mediator.PublishAsync(new TestEvent());
        
        var requestResponse = await mediator.RequestAsync<TestRequest, TestResponse>(new TestRequest());
        var commandResponse = await mediator.SendAsync<TestCommand, TestResponse>(new TestCommand(Guid.NewGuid()));
        
        requestResponse.Result.ShouldBe("Test response");
        commandResponse.Result.ShouldBe("Test command response");
    }
}