using System;
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
                .RegisterHandlers()
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
}