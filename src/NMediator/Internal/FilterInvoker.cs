using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Internal;

public class FilterInvoker<TMessage> where TMessage : class, IMessage
{
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
}