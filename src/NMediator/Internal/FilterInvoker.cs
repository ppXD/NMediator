using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using NMediator.Filters;

namespace NMediator.Internal;

public class FilterInvoker<TMessage> where TMessage : class, IMessage
{
    private readonly TMessage _message;
    private readonly IDependencyScope _scope;
    private readonly IList<IFilter> _filters;

    public FilterInvoker(TMessage message, IDependencyScope scope, IList<IFilter> filters)
    {
        _message = message;
        _scope = scope;
        _filters = filters;
    }

    public async Task<HandlerExecutedContext<TMessage>> InvokeHandlerFilter(
        dynamic filter, HandlerExecutingContext<TMessage> preContext, 
        Func<Task<HandlerExecutedContext<TMessage>>> continuation, CancellationToken cancellationToken)
    {
        await filter.OnHandlerExecuting(preContext, cancellationToken).ConfigureAwait(false);
        
        if (preContext.Result != null)
        {
            return new HandlerExecutedContext<TMessage>(preContext.Message, preContext.Scope, preContext.Filters)
            {
                Result = preContext.Result
            };
        }
        
        var wasError = false;
        HandlerExecutedContext<TMessage> postContext;

        try
        {
            postContext = await continuation().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            wasError = true;

            postContext = new HandlerExecutedContext<TMessage>(preContext.Message, preContext.Scope, preContext.Filters)
            {
                ExceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex)
            };

            await filter.OnHandlerExecuted(postContext, cancellationToken).ConfigureAwait(false);

            if (!postContext.ExceptionHandled)
                throw;
        }

        if (!wasError)
            await filter.OnHandlerExecuted(postContext, cancellationToken).ConfigureAwait(false);

        return postContext;
    }

    public async Task<ExceptionContext<TMessage>> InvokeExceptionFilters(
        IEnumerable<dynamic> filters, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionContext = new ExceptionContext<TMessage>(_message, _scope, _filters)
        {
            ExceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception)
        };

        foreach (var filter in filters)
        {
            await filter.OnException(exceptionContext, cancellationToken);
            
            if (exceptionContext.ExceptionHandled) break;
        }
        
        return exceptionContext;
    }
}