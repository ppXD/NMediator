using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using NMediator.Filters;

namespace NMediator.Internal;

public class MessageProcessor<TMessage> where TMessage : class, IMessage, new()
{
    private readonly TMessage _message;
    private readonly IDependencyScope _scope;
    private readonly IList<IFilter> _filters;

    private readonly FilterInvoker<TMessage> _filterInvoker;
    private readonly HandlerInvoker<TMessage> _handlerInvoker;

    public MessageProcessor(
        TMessage message, 
        IDependencyScope scope, 
        IList<IFilter> filters, 
        IList<HandlerWrapper> handlers)
    {
        _message = message;
        _scope = scope;
        _filters = filters;

        _filterInvoker = new FilterInvoker<TMessage>(message, scope, filters);
        _handlerInvoker = new HandlerInvoker<TMessage>(message, scope, handlers);
    }

    public async Task<HandlerExecutedContext<TMessage>> Process(CancellationToken cancellationToken)
    {
        async Task<HandlerExecutedContext<TMessage>> Continuation() => new(_message, _scope, _filters)
        {
            Result = await _handlerInvoker.Invoke(cancellationToken).ConfigureAwait(false)
        };
        
        var preContext = 
            new HandlerExecutingContext<TMessage>(_message, _scope, _filters);

        var thunk = GetHandlerFilters().Reverse().Aggregate(
            (Func<Task<HandlerExecutedContext<TMessage>>>)Continuation, (next, filter) => async () =>
                await _filterInvoker.InvokeHandlerFilter(filter, preContext, next, cancellationToken)
                    .ConfigureAwait(false));

        try
        {
            return await thunk().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            var exceptionContext = await _filterInvoker
                .InvokeExceptionFilters(GetExceptionFilters(), ex, cancellationToken).ConfigureAwait(false);
            
            if (!exceptionContext.ExceptionHandled) throw;
        }

        return null;
    }

    private IEnumerable<IFilter> GetHandlerFilters()
    {
        return GetFilters(typeof(IHandlerFilter<>));
    }

    private IEnumerable<IFilter> GetExceptionFilters()
    {
        return GetFilters(typeof(IExceptionFilter));
    }

    private IEnumerable<IFilter> GetFilters(Type targetFilter)
    {
        return _filters
            .Where(filter =>
            {
                var filterType = filter switch
                {
                    TypeFilter typeFilter => typeFilter.ImplementationType,
                    _ => filter.GetType()
                };
                return filterType.GetInterfaces()
                    .Any(x => x == targetFilter || x.IsGenericType &&
                              x.GetGenericTypeDefinition() == targetFilter);
            })
            .Select(filter =>
            {
                if (filter is TypeFilter typeFilter)
                    return (IFilter)_scope.Resolve(typeFilter.ImplementationType);
                return filter;
            });
    }
}