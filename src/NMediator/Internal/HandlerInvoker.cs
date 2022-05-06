using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NMediator.Internal;

public class HandlerInvoker<TMessage> where TMessage : class, IMessage
{
    public async Task<object> Invoke(TMessage message, IList<HandlerWrapper> handlers, IDependencyScope scope, CancellationToken cancellationToken)
    {
        if (handlers == null || !handlers.Any())
            throw new NoHandlerFoundException(typeof(TMessage));

        if (message is ICommand or IRequest)
            return await InvokeHandleMethod(message, handlers.First(), scope, cancellationToken)
                .ConfigureAwait(false);

        foreach (var handler in handlers)
            await InvokeHandleMethod(message, handler, scope, cancellationToken, false)
                .ConfigureAwait(false);

        return null;
    }

    private static async Task<object> InvokeHandleMethod(TMessage message, HandlerWrapper handlerWrapper, IDependencyScope scope, CancellationToken cancellationToken, bool shouldGetResult = true)
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