using System;
using System.Linq;
using System.Threading.Tasks;
using NMediator.Test.TestData;
using NMediator.Test.TestData.CommandHandlers;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Requests;
using Shouldly;
using Xunit;

namespace NMediator.Test;

public class CommandFixture : TestBase
{
    [Fact]
    public async Task ShouldCommandHandled()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .CreateMediator();

        var command = new TestCommand(Guid.NewGuid());
            
        await mediator.SendAsync(command);
            
        TestStore.Stores.Count.ShouldBe(1);
        TestStore.Stores.Single().ShouldBe(command);
    }

    [Fact]
    public async Task ShouldHasResponseCommandHandlerBeWork()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHasResponseHandler>()
            .CreateMediator();

        var command = new TestCommand(Guid.NewGuid());
            
        await mediator.SendAsync(command);
        var response = await mediator.SendAsync<TestCommand, TestResponse>(new TestCommand(Guid.NewGuid()));
            
        response.ShouldNotBeNull();
        TestStore.Stores.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldCommandSendToItsHandler()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandlers(typeof(TestCommandHandler), typeof(TestOtherCommandHandler))
            .CreateMediator();

        var command = new TestOtherCommand();
            
        await mediator.SendAsync(command);
            
        TestStore.Stores.Count.ShouldBe(1);
        TestStore.Stores.Single().ShouldBe(command);
    }

    [Fact]
    public async Task ShouldSendMultipleCommand()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .RegisterHandler<TestOtherCommandHandler>()
            .CreateMediator();

        var command1 = new TestCommand(Guid.NewGuid());
        var command2 = new TestOtherCommand();
            
        await mediator.SendAsync(command1);
        await mediator.SendAsync(command2);
            
        TestStore.Stores.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldOneHandlerToHandleMultipleCommands()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestMultipleCommandHandler>()
            .CreateMediator();

        var command1 = new TestCommand(Guid.NewGuid());
        var command2 = new TestOtherCommand();
            
        await mediator.SendAsync(command1);
        await mediator.SendAsync(command2);
        var response = await mediator.SendAsync<TestOtherCommand, TestResponse>(command2);
            
        response.ShouldNotBeNull();
        TestStore.Stores.Count.ShouldBe(3);
    }

    [Fact]
    public async Task ShouldNotDuplicatedHandlersWhenRegisterSameHandlers()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .RegisterHandler<TestCommandHandler>()
            .CreateMediator();

        var command1 = new TestCommand(Guid.NewGuid());
            
        await mediator.SendAsync(command1);
        
        TestStore.Stores.Count.ShouldBe(1);
    }
    
    [Fact]
    public void ShouldNotHandleUnMatchedResponse()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestMultipleCommandHandler>()
            .CreateMediator();

        var command = new TestCommand(Guid.NewGuid());
            
        Func<Task> sendTask = () => mediator.SendAsync<TestCommand, TestOtherResponse>(command);

        sendTask.ShouldThrow<NoHandlerFoundException>();
    }
    
    [Fact]
    public void CannotMoreThanOneCommandHandler()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .RegisterHandler<TestCommandHasResponseHandler>()
            .CreateMediator();
            
        var command = new TestCommand(Guid.NewGuid());

        var sendTask1 = () => mediator.SendAsync(command);
        var sendTask2 = () => mediator.SendAsync<TestCommand, TestResponse>(command);

        sendTask1.ShouldThrow<MoreThanOneHandlerException>();
        sendTask2.ShouldThrow<MoreThanOneHandlerException>();
    }
}