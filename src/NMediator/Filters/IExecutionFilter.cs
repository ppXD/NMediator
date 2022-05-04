using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator.Filters;

public interface IExecutionFilter<TMessage, in TContext> : IFilter
    where TMessage : class, IMessage
{
    Task OnExecuting(TContext context, CancellationToken cancellationToken = default);
    
    Task OnExecuted(TContext context, CancellationToken cancellationToken = default);
}