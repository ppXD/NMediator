using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public interface IMiddleware
    {
        Task InvokeAsync(object message, CancellationToken cancellationToken = default);
    }
}