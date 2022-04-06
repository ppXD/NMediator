using System.Threading;
using System.Threading.Tasks;

namespace NMediator.Middleware;

public class HandlerInvokerMiddleware : IMiddleware
{
    public Task OnExecuting(object message, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task OnExecuted(object message, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}