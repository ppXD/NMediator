using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NMediator.Test
{
    public class UnitTest
    {
        [Fact]
        public async Task Test()
        {
            var mediator = new MediatorConfiguration()
                //.RegisterHandler(typeof(TestCommandHandler))
                //.RegisterHandler<TestCommandHandler1>()
                .RegisterHandlers(typeof(UnitTest).Assembly)
                .UseMiddleware<TestMiddleware>()
                .UseMiddleware(next =>
                {
                    return async message =>
                    {
                        await next(message);
                    };
                })
                .UseMiddleware(next =>
                {
                    return async message =>
                    {
                        await next(message);
                    };
                })
                .CreateMediator();

            await mediator.SendAsync(new TestCommand());
        }
    }

    public class TestMiddleware : IMiddleware
    {
        public Task InvokeAsync(object message, MessageDelegate next)
        {
            if (message is TestCommand testCommand)
            {
                testCommand.Id = Guid.NewGuid();
            }
            
            return next.Invoke(message);
        }
    }

    public class TestCommand : ICommand
    {
        public Guid Id { get; set; }
    }
    
    public class TestCommand1 : ICommand
    {
        public Guid Id { get; set; }
    }
    
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task Handle(TestCommand message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
    
    public class TestCommandHandler1 : ICommandHandler<TestCommand>, ICommandHandler<TestCommand1>
    {
        public Task Handle(TestCommand message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Handle(TestCommand1 command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}