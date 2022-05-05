using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using NMediator.Filters;

namespace NMediator.Internal;

public class MessageProcessor<TMessage> where TMessage : class, IMessage
{
    private readonly TMessage _message;
    private readonly IDependencyScope _scope;
    private readonly IList<IFilter> _filters;
    private readonly IList<HandlerWrapper> _handlers;

    private readonly FilterInvoker<TMessage> _filterInvoker =  new();
    private readonly HandlerInvoker<TMessage> _handlerInvoker = new();

    public MessageProcessor(
        TMessage message, 
        IDependencyScope scope, 
        IList<IFilter> filters, 
        IList<HandlerWrapper> handlers)
    {
        _message = message;
        _scope = scope;
        _filters = filters;
        _handlers = handlers;
    }

    public async Task Process(CancellationToken cancellationToken = default)
    {
        async Task<HandlerExecutedContext<TMessage>> Continuation() => new(_message, _scope, _filters)
        {
            Result = await _handlerInvoker.Invoke(_message, _handlers, _scope, cancellationToken).ConfigureAwait(false)
        };
        
        var preContext = 
            new HandlerExecutingContext<TMessage>(_message, _scope, _filters);

        var thunk = GetHandlerFilters().Reverse().Aggregate(
            (Func<Task<HandlerExecutedContext<TMessage>>>)Continuation, (next, filter) => async () =>
                await _filterInvoker.InvokeHandlerFilter(filter, preContext, next, cancellationToken)
                    .ConfigureAwait(false));

        await thunk();
    }

    private IEnumerable<IFilter> GetHandlerFilters()
    {
        return _filters
            .Where(filter => filter is IHandlerFilter<IMessage> || 
                             filter is TypeFilter t && t.ImplementationType.GetInterfaces()
                                 .Any(x => x.IsGenericType &&
                                           x.GetGenericTypeDefinition() == typeof(IHandlerFilter<>)))
            .Select(filter =>
            {
                if (filter is TypeFilter typeFilter)
                    return (IFilter)_scope.Resolve(typeFilter.ImplementationType);
                return filter;
            });
    }
}