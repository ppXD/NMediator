using System;
using System.Linq;
using System.Threading.Tasks;
using NMediator.Test.TestData;
using NMediator.Test.TestData.CommandHandlers;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Responses;
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
            .RegisterHandler<TestStringCommandHandler>()
            .CreateMediator();

        var normalCommand = new TestCommand();
        var stringCommand = new TestStringCommand();
        
        await mediator.SendAsync(normalCommand);
        var response = await mediator.SendAsync(stringCommand);

        response.ShouldNotBeNull();
        
        TestStore.Stores.Count.ShouldBe(2);
        TestStore.Stores.Any(x => x == normalCommand).ShouldBeTrue();
        TestStore.Stores.Any(x => x == stringCommand).ShouldBeTrue();
    }

    [Fact]
    public async Task ShouldHasResponseCommandHandlerBeWork()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .RegisterHandler<TestHasResponseCommandHandler>()
            .CreateMediator();

        var command = new TestCommand();
        
        await mediator.SendAsync(command);
        var response = await mediator.SendAsync(new TestHasResponseCommand());
        
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

        var command1 = new TestCommand();
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

        var command1 = new TestCommand();
        var command2 = new TestHasResponseCommand();
            
        await mediator.SendAsync(command1);
        await mediator.SendAsync(command2);
        var response = await mediator.SendAsync(command2);
        
        response.ShouldNotBeNull();
        TestStore.Stores.Count.ShouldBe(3);
    }

    [Fact]
    public async Task ShouldDerivedResponseCommandBeWork()
    {
        var mediator1 = new MediatorConfiguration()
            .RegisterHandler<TestHasDerivedResponseCommandHandler1>()
            .CreateMediator();
        
        var mediator2 = new MediatorConfiguration()
            .RegisterHandler<TestHasDerivedResponseCommandHandler2>()
            .CreateMediator();
        
        var mediator3 = new MediatorConfiguration()
            .RegisterHandler<TestHasDerivedResponseCommandHandler3>()
            .CreateMediator();
        
        var response1 = await mediator1.SendAsync(new TestHasDerivedResponseCommand());
        var response2 = await mediator1.SendAsync<TestResponse>(new TestHasDerivedResponseCommand());

        var response3 = () => mediator2.SendAsync(new TestHasDerivedResponseCommand());
        var response4 = await mediator2.SendAsync<TestResponse>(new TestHasDerivedResponseCommand());
        
        var response5 = () => mediator3.SendAsync(new TestHasDerivedResponseCommand());
        var response6 = await mediator3.SendAsync<TestResponse>(new TestHasDerivedResponseCommand());
        
        response1.ShouldNotBeNull();
        response2.ShouldNotBeNull();
        response3.ShouldThrow<NoHandlerFoundException>();
        response4.ShouldNotBeNull();
        response5.ShouldThrow<Exception>();
        response6.ShouldNotBeNull();
        TestStore.Stores.Count.ShouldBe(5);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ShouldMultipleImplementationCommandSingleRegisterWork(bool hasResponse)
    {
        var config = new MediatorConfiguration();

        if (hasResponse)
            config.RegisterHandler<TestMultipleImplementationHasResponseCommandHandler>();
        else
            config.RegisterHandler<TestMultipleImplementationNonResponseCommandHandler1>();

        var mediator = config.CreateMediator();
        
        var command = new TestMultipleImplementationCommand();

        if (hasResponse)
        {
            await mediator.SendAsync<TestResponse>(command);
            
            var otherResponseTask = () => mediator.SendAsync<TestOtherResponse>(command);

            otherResponseTask.ShouldThrow<NoHandlerFoundException>();
        }
        else
            await mediator.SendAsync(command);
        
        TestStore.Stores.Count.ShouldBe(1);
    }

    [Fact]
    public async Task ShouldInheritHandlerBeWork()
    {
        var mediator1 = new MediatorConfiguration()
            .RegisterHandler<ITestCommandHandler>()
            .RegisterHandler<ITestCommandHasResponseHandler>()
            .CreateMediator();

        var mediator2 = new MediatorConfiguration()
            .RegisterHandler<ITestCommandHandler>()
            .RegisterHandler<TestInterfaceCommandHandler>()
            .RegisterHandler<TestInterfaceHasResponseCommandHandler>()
            .CreateMediator();
        
        var mediator3 = new MediatorConfiguration()
            .RegisterHandler<ITestCommandHandler>()
            .RegisterHandler<TestCommandOneWayCommandHandler>()
            .RegisterHandler<TestCommandOneWayHasResponseCommandHandler>()
            .CreateMediator();
        
        var mediator4 = new MediatorConfiguration()
            .RegisterHandler<ITestCommandHandler>()
            .RegisterHandler<TestCommandOneWayCommandHandler>()
            .RegisterHandler<TestCommandOneWayHasResponseCommandHandler>()
            .RegisterHandler<TestCommandTwoWayCommandHandler>()
            .RegisterHandler<TestCommandTwoWayHasResponseCommandHandler>()
            .CreateMediator();
        
        var mediator5 = new MediatorConfiguration()
            .RegisterHandler<ITestCommandHandler>()
            .RegisterHandler<TestCommandOneWayCommandHandler>()
            .RegisterHandler<TestCommandOneWayHasResponseCommandHandler>()
            .RegisterHandler<TestCommandTwoWayCommandHandler>()
            .RegisterHandler<TestCommandTwoWayHasResponseCommandHandler>()
            .RegisterHandler<TestCommandAllWayCommandHandler>()
            .RegisterHandler<TestCommandAllWayHasResponseCommandHandler>()
            .CreateMediator();
        
        var mediator6 = new MediatorConfiguration()
            .RegisterHandler<ITestCommandHandler>()
            .RegisterHandler<TestCommandAllWayCommandHandler>()
            .RegisterHandler<TestCommandAllWayHasResponseCommandHandler>()
            .RegisterHandler<TestInheritAllWayCommandHandler>()
            .RegisterHandler<TestInheritAllWayHasResponseCommandHandler>()
            .CreateMediator();
        
         await mediator1.SendAsync(new TestInterfaceCommand());
         await mediator1.SendAsync<TestResponse>(new TestInterfaceCommand());
         await mediator1.SendAsync<TestDerivedResponse>(new TestInterfaceCommand());
         await mediator2.SendAsync(new TestInterfaceCommand());
         await mediator2.SendAsync<TestResponse>(new TestInterfaceCommand());
         await mediator2.SendAsync<TestDerivedResponse>(new TestInterfaceCommand());
        
         TestStore.Stores[0].ShouldBe($"{nameof(ITestCommandHandler)}");
         TestStore.Stores[1].ShouldBe($"{nameof(ITestCommandHasResponseHandler)}");
         TestStore.Stores[2].ShouldBe($"{nameof(ITestCommandHasResponseHandler)}");
         TestStore.Stores[3].ShouldBe($"{nameof(TestInterfaceCommandHandler)}");
         TestStore.Stores[4].ShouldBe($"{nameof(TestInterfaceHasResponseCommandHandler)}");
         TestStore.Stores[5].ShouldBe($"{nameof(TestInterfaceHasResponseCommandHandler)}");
         TestStore.Stores.Clear();
        
         await mediator1.SendAsync(new TestCommandOneWayCommand());
         await mediator1.SendAsync<TestResponse>(new TestCommandOneWayCommand());
         await mediator1.SendAsync<TestDerivedResponse>(new TestCommandOneWayCommand());
         await mediator3.SendAsync(new TestCommandOneWayCommand());
         await mediator3.SendAsync<TestResponse>(new TestCommandOneWayCommand());
         await mediator3.SendAsync<TestDerivedResponse>(new TestCommandOneWayCommand());
        
         TestStore.Stores[0].ShouldBe($"{nameof(ITestCommandHandler)}");
         TestStore.Stores[1].ShouldBe($"{nameof(ITestCommandHasResponseHandler)}");
         TestStore.Stores[2].ShouldBe($"{nameof(ITestCommandHasResponseHandler)}");
         TestStore.Stores[3].ShouldBe($"{nameof(TestCommandOneWayCommandHandler)}");
         TestStore.Stores[4].ShouldBe($"{nameof(TestCommandOneWayHasResponseCommandHandler)}");
         TestStore.Stores[5].ShouldBe($"{nameof(TestCommandOneWayHasResponseCommandHandler)}");
         TestStore.Stores.Clear();
        
         await mediator1.SendAsync(new TestCommandTwoWayCommand());
         await mediator1.SendAsync<TestResponse>(new TestCommandTwoWayCommand());
         await mediator1.SendAsync<TestDerivedResponse>(new TestCommandTwoWayCommand());
         await mediator4.SendAsync(new TestCommandTwoWayCommand());
         await mediator4.SendAsync<TestResponse>(new TestCommandTwoWayCommand());
         await mediator4.SendAsync<TestDerivedResponse>(new TestCommandTwoWayCommand());
        
         TestStore.Stores[0].ShouldBe($"{nameof(ITestCommandHandler)}");
         TestStore.Stores[1].ShouldBe($"{nameof(ITestCommandHasResponseHandler)}");
         TestStore.Stores[2].ShouldBe($"{nameof(ITestCommandHasResponseHandler)}");
         TestStore.Stores[3].ShouldBe($"{nameof(TestCommandTwoWayCommandHandler)}");
         TestStore.Stores[4].ShouldBe($"{nameof(TestCommandTwoWayHasResponseCommandHandler)}");
         TestStore.Stores[5].ShouldBe($"{nameof(TestCommandTwoWayHasResponseCommandHandler)}");
         TestStore.Stores.Clear();
        
         await mediator1.SendAsync(new TestInheritAllWayCommand());
         await mediator1.SendAsync<TestResponse>(new TestInheritAllWayCommand());
         await mediator1.SendAsync<TestDerivedResponse>(new TestInheritAllWayCommand());
         await mediator5.SendAsync(new TestInheritAllWayCommand());
         await mediator5.SendAsync<TestResponse>(new TestInheritAllWayCommand());
         await mediator5.SendAsync<TestDerivedResponse>(new TestInheritAllWayCommand());
        await mediator6.SendAsync(new TestInheritAllWayCommand());
        await mediator6.SendAsync<TestResponse>(new TestInheritAllWayCommand());
        await mediator6.SendAsync<TestDerivedResponse>(new TestInheritAllWayCommand());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestCommandHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestCommandHasResponseHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(ITestCommandHasResponseHandler)}");
        TestStore.Stores[3].ShouldBe($"{nameof(TestCommandAllWayCommandHandler)}");
        TestStore.Stores[4].ShouldBe($"{nameof(TestCommandAllWayHasResponseCommandHandler)}");
        TestStore.Stores[5].ShouldBe($"{nameof(TestCommandAllWayHasResponseCommandHandler)}");
        TestStore.Stores[6].ShouldBe($"{nameof(TestInheritAllWayCommandHandler)}");
        TestStore.Stores[7].ShouldBe($"{nameof(TestInheritAllWayHasResponseCommandHandler)}");
        TestStore.Stores[8].ShouldBe($"{nameof(TestInheritAllWayHasResponseCommandHandler)}");
    }

    [Fact]
    public async Task ShouldAbstractHandlerBeWork()
    {
        var mediator1 = new MediatorConfiguration()
            .RegisterHandler<ITestAbstractCommandHandler>()
            .RegisterHandler<ITestAbstractHasResponseCommandHandler>()
            .CreateMediator();
        
        var mediator2 = new MediatorConfiguration()
            .RegisterHandler<ITestAbstractCommandHandler>()
            .RegisterHandler<TestAbstractCommandBaseHandler>()
            .RegisterHandler<TestAbstractHasResponseCommandBaseHandler>()
            .CreateMediator();
        
        var mediator3 = new MediatorConfiguration()
            .RegisterHandler<ITestAbstractCommandHandler>()
            .RegisterHandler<TestAbstractCommandBaseHandler>()
            .RegisterHandler<TestAbstractHasResponseCommandBaseHandler>()
            .RegisterHandler<TestAbstractCommandHandler>()
            .RegisterHandler<TestAbstractHasResponseCommandHandler>()
            .CreateMediator();
        
        await mediator1.SendAsync(new TestAbstractCommand());
        await mediator1.SendAsync<TestResponse>(new TestAbstractCommand());
        await mediator1.SendAsync<TestDerivedResponse>(new TestAbstractCommand());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestAbstractCommandHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestAbstractHasResponseCommandHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(ITestAbstractHasResponseCommandHandler)}");
        TestStore.Stores.Clear();
        
        await mediator2.SendAsync(new TestAbstractCommand());
        await mediator2.SendAsync<TestResponse>(new TestAbstractCommand());
        await mediator2.SendAsync<TestDerivedResponse>(new TestAbstractCommand());
        
        TestStore.Stores[0].ShouldBe($"{nameof(TestAbstractCommandBaseHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestAbstractHasResponseCommandBaseHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(TestAbstractHasResponseCommandBaseHandler)}");
        TestStore.Stores.Clear();
        
        await mediator3.SendAsync(new TestAbstractCommand());
        await mediator3.SendAsync<TestResponse>(new TestAbstractCommand());
        await mediator3.SendAsync<TestDerivedResponse>(new TestAbstractCommand());
        
        TestStore.Stores[0].ShouldBe($"{nameof(TestAbstractCommandHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestAbstractHasResponseCommandHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(TestAbstractHasResponseCommandHandler)}");
    }
    
    [Fact]
    public async Task ShouldNotDuplicatedHandlersWhenRegisterSameHandlers()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .RegisterHandler<TestCommandHandler>()
            .RegisterHandler<TestHasResponseCommandHandler>()
            .CreateMediator();

        await mediator.SendAsync(new TestCommand());
        await mediator.SendAsync(new TestHasResponseCommand());
        
        TestStore.Stores.Count.ShouldBe(2);
    }

    [Fact]
    public async Task DuplicatedHandlerShouldNotThrow()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestMultipleImplementationNonResponseCommandHandler1>()
            .RegisterHandler<TestMultipleImplementationNonResponseCommandHandler2>()
            .RegisterHandler<TestMultipleImplementationHasResponseCommandHandler>()
            .CreateMediator();
        
        var command = new TestMultipleImplementationCommand();
        
        await mediator.SendAsync(command);
        var response2 = await mediator.SendAsync<TestResponse>(command);

        response2.ShouldNotBeNull();
        TestStore.Stores.Count.ShouldBe(2);
        TestStore.Stores[0].ShouldBe($"{nameof(TestMultipleImplementationNonResponseCommandHandler1)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestMultipleImplementationHasResponseCommandHandler)}");
    }
}