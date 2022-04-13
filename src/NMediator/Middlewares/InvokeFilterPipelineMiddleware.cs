using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using NMediator.Context;
using NMediator.Filters;
using NMediator.Infrastructure;

namespace NMediator.Middlewares;

public class InvokeFilterPipelineMiddleware : IMiddleware
{
    public async Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        var filterInvoker = new FilterInvoker(context);
        var handlerInvoker = new HandlerInvoker(context);
        
        var executionFilters = GetFilters(context.Filters, typeof(IExecutionFilter<,>));

        var thunk = executionFilters.Reverse()
            .Aggregate((Func<Task>)Continuation,
                (next, filter) => async () =>
                    await filterInvoker.Invoke(filter, next, cancellationToken).ConfigureAwait(false));

        async Task Continuation() =>
            context.Result = await handlerInvoker.Invoke(cancellationToken).ConfigureAwait(false);
        
        await thunk().ConfigureAwait(false);
    }
    
    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    private static IEnumerable<Type> GetFilters(IEnumerable<Type> filters, Type filterType)
    {
        return filters.Where(f =>
            f.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == filterType));
    }
}