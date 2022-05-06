using System.Threading;
using System.Threading.Tasks;

namespace NMediator.Filters;

public interface IHandlerFilter<in TMessage> : IFilter
    where TMessage : class, IMessage
{
    Task OnHandlerExecuting(IHandlerExecutingContext<TMessage> context, CancellationToken cancellationToken = default);
    
    Task OnHandlerExecuted(IHandlerExecutedContext<TMessage> context, CancellationToken cancellationToken = default);
}