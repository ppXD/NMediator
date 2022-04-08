using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator.Middlewares;

public interface IMiddleware
{
    Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default);
        
    Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default);
}