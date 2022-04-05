using System.Threading;
using System.Threading.Tasks;
using NMediator.Examples.AspNetCore.Commands;

namespace NMediator.Examples.AspNetCore.Middlewares
{
    public class TestMiddleware : IMiddleware
    {
        public async Task InvokeAsync(object message, CancellationToken cancellationToken)
        {
            if (message is TestCommand command)
            {
                command.Message = "Invoked middleware";
            }
        }
    }
}