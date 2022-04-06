using System.Threading;
using System.Threading.Tasks;

namespace NMediator.Middleware
{
    public interface IMiddleware
    {
        Task OnExecuting(object message, CancellationToken cancellationToken = default);
        
        Task OnExecuted(object message, CancellationToken cancellationToken = default);
    }
}