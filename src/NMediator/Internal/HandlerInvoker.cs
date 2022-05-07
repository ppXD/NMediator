using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NMediator.Internal;

public class HandlerInvoker<TMessage> where TMessage : class, IMessage
{
    private readonly TMessage _message;
    private readonly IDependencyScope _scope;
    private readonly IList<HandlerWrapper> _handlers;

    public HandlerInvoker(TMessage message, IDependencyScope scope, IList<HandlerWrapper> handlers)
    {
        _message = message;
        _scope = scope;
        _handlers = handlers;
    }

    public async Task<object> Invoke(CancellationToken cancellationToken)
    {
        if (_handlers == null || !_handlers.Any())
            throw new NoHandlerFoundException(typeof(TMessage));

        if (_message is ICommand or IRequest)
            return await InvokeHandleMethod(_message, _handlers.First(), _scope, cancellationToken)
                .ConfigureAwait(false);

        foreach (var handler in _handlers)
            await InvokeHandleMethod(_message, handler, _scope, cancellationToken, false)
                .ConfigureAwait(false);

        return null;
    }

    private static async Task<object> InvokeHandleMethod(
        TMessage message, HandlerWrapper handlerWrapper, 
        IDependencyScope scope, CancellationToken cancellationToken, bool shouldGetResult = true)
    {
        var handler = scope.Resolve(handlerWrapper.Handler) as dynamic;
        var handleTask = (Task) handler.Handle(message, cancellationToken);
        await handleTask.ConfigureAwait(false);
        return shouldGetResult ? GetResultFromTask(handleTask) : null;
    }
    
    private static object GetResultFromTask(Task task)
    {
        if (!task.GetType().GetTypeInfo().IsGenericType)
            return null;
        var result = task.GetType().GetRuntimeProperty("Result")?.GetMethod;
        return result?.Invoke(task, Array.Empty<object>());
    }
}