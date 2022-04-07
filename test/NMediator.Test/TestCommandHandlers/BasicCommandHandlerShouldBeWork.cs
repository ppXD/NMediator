using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Middleware;
using NMediator.Test.TestData;
using Xunit;
using NMediator.Test.TestData.CommandHandlers;
using NMediator.Test.TestData.Commands;
using Shouldly;

namespace NMediator.Test.TestCommandHandlers
{
    public class BasicCommandHandlerShouldBeWork : TestBase
    {
        [Fact]
        public async Task Test()
        {
            var mediator = new MediatorConfiguration()
                .RegisterHandler<TestCommandHandler>()
                .UseMiddleware<TestMiddleware>()
                .UseMiddleware<TestMiddleware2>()
                .CreateMediator();

            var command = new TestCommand(Guid.NewGuid());
            
            await mediator.SendAsync(command);
            
            TestStore.CommandStore.Count.ShouldBe(1);
            TestStore.CommandStore.Single().ShouldBe(command);
        }
    }
    
    public class TestMiddleware : IMiddleware
    {
        public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
        {
            if (context.Message is TestCommand command)
            {
                command.Name = "1";
            }
            
            return Task.CompletedTask;
        }

        public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
        {
            if (context.Message is TestCommand command)
            {
                command.Name = "2";
            }
            
            return Task.CompletedTask;
        }
    }
    public class TestMiddleware2 : IMiddleware
    {
        public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
        {
            if (context.Message is TestCommand command)
            {
                command.Name = "3";
            }
            
            return Task.CompletedTask;
        }

        public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
        {
            if (context.Message is TestCommand command)
            {
                command.Name = "4";
            }
            
            return Task.CompletedTask;
        }
    }
}