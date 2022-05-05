using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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
        var handler = scope.Resolve(handlerWrapper.Handler);
        var handleMethod = GetHandleMethod(handlerWrapper.Handler, typeof(TMessage), handlerWrapper.ResponseType);

        var handleTask = (Task)handleMethod.Invoke(handler, new object[] { message, cancellationToken });

        if (handleTask == null) return null;

        await handleTask.ConfigureAwait(false);

        return shouldGetResult ? GetResultFromTask(handleTask) : null;
    }

    private static MethodInfo GetHandleMethod(Type handlerType, Type messageType, Type responseType)
    {
        return handlerType.GetRuntimeMethods()
            .Where(m => IsHandleMethod(m, messageType, responseType))
            .OrderBy(m => Prioritize(m, messageType))
            .First();
    }

    private static bool IsHandleMethod(MethodInfo m, Type messageType, Type responseType)
    {
        return m.Name == "Handle" && m.IsPublic && ContainsReturnType(m, responseType) && ContainsParameter(m, messageType);
    }

    private static bool ContainsReturnType(MethodInfo m, Type returnType)
    {
        if (returnType != null)
            return m.ReturnType.GetGenericArguments().Contains(returnType);

        return m.ReturnType == typeof(Task);
    }

    private static bool ContainsParameter(MethodBase m, Type messageType)
    {
        var handleMessageType = m.GetParameters()[0].ParameterType;
        
        return handleMessageType == messageType || handleMessageType.IsAssignableFrom(messageType);
    }

    private static int Prioritize(MethodBase m, Type messageType)
    {
        var handleMessageType = m.GetParameters()[0].ParameterType;

        if (handleMessageType == messageType)
            return 1;
        return handleMessageType.IsSubclassOf(messageType) ? 2 : 3;
    }
    
    private static object GetResultFromTask(Task task)
    {
        if (!task.GetType().GetTypeInfo().IsGenericType)
            return null;
        var result = task.GetType().GetRuntimeProperty("Result")?.GetMethod;
        return result?.Invoke(task, Array.Empty<object>());
    }
}