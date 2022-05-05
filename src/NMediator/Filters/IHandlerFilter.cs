using System.Threading;
using System.Threading.Tasks;

namespace NMediator.Filters;

public interface IHandlerFilter<TMessage> :
    IHandlerFilter<TMessage, HandlerExecutingContext<TMessage>, HandlerExecutedContext<TMessage>>
    where TMessage : class, IMessage
{
}

public interface IHandlerFilter<TMessage, in THandlerExecutingContext, in THandlerExecutedContext> : IFilter
    where THandlerExecutingContext : FilterContext<TMessage>
    where THandlerExecutedContext : FilterContext<TMessage>
    where TMessage : class, IMessage
{
    Task OnHandlerExecuting(THandlerExecutingContext context, CancellationToken cancellationToken = default);
    
    Task OnHandlerExecuted(THandlerExecutedContext context, CancellationToken cancellationToken = default);
}