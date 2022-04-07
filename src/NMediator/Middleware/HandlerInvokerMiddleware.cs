using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator.Middleware;

public class HandlerInvokerMiddleware : IMiddleware
{
    public async Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        if (context.MessageBindingHandlers == null || !context.MessageBindingHandlers.Any())
            throw new NoHandlerFoundException(context.Message.GetType());

        if (context.Message is ICommand or IRequest && context.MessageBindingHandlers.Count() > 1)
            throw new MoreThanOneHandlerException(context.Message.GetType());

        foreach (var handlerType in context.MessageBindingHandlers)
        {
            var handleMethod = GetHandleMethod(handlerType, context.Message.GetType());
            var handler = context.Scope.Resolve(handlerType);
            
            var handleTask = (Task) handleMethod.Invoke(handler, new object[] { context, cancellationToken });

            await handleTask.ConfigureAwait(false);

            context.Result = GetResultFromTask(handleTask);
        }
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    private static MethodInfo GetHandleMethod(Type handlerType, Type messageType)
    {
        return handlerType.GetRuntimeMethods().Single(m => IsHandleMethod(m, messageType));
    }

    private static bool IsHandleMethod(MethodBase m, Type messageType)
    {
        return m.Name == "Handle" && m.IsPublic && m.GetParameters().Any()
               && (m.GetParameters()[0].ParameterType.GenericTypeArguments.Contains(messageType) || m.GetParameters()[0]
                   .ParameterType.GenericTypeArguments.First().GetTypeInfo()
                   .IsAssignableFrom(messageType.GetTypeInfo()));
    }
    
    private static object? GetResultFromTask(Task task)
    {
        if (!task.GetType().GetTypeInfo().IsGenericType)
            return null;
        var result = task.GetType().GetRuntimeProperty("Result").GetMethod;
        return result.Invoke(task, new object[] { });
    }
}