using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator.Middlewares;

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
            var handleMethod = GetHandleMethod(handlerType, context.Message.GetType(), context.ResponseType);
            var handleMethodContextType = GetHandleMethodContextType(handleMethod, context.Message.GetType());
            var handler = context.Scope.Resolve(handlerType);

            var handleContext = Activator.CreateInstance(handleMethodContextType, context);
            
            var handleTask = (Task) handleMethod.Invoke(handler, new[] { handleContext, cancellationToken });

            await handleTask.ConfigureAwait(false);

            context.Result = GetResultFromTask(handleTask);
        }
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    private static MethodInfo GetHandleMethod(Type handlerType, Type messageType, Type responseType)
    {
        return handlerType.GetRuntimeMethods().Single(m => IsHandleMethod(m, messageType, responseType));
    }

    private static bool IsHandleMethod(MethodInfo m, Type messageType, Type responseType)
    {
        return m.Name == "Handle" && m.IsPublic && ContainsReturnType(m, messageType, responseType) && ContainsParameter(m, messageType);
    }

    private static bool ContainsReturnType(MethodInfo m, Type messageType, Type returnType)
    {
        if (returnType != null)
            return m.ReturnType.GetGenericArguments().Contains(returnType);
        
        // Only contains parameter when using mediator.SendAsync();
        if (typeof(ICommand).IsAssignableFrom(messageType))
            return true;
        
        return m.ReturnType == typeof(Task);
    }

    private static bool ContainsParameter(MethodBase m, Type messageType)
    {
        return m.GetParameters().Any()
               && (m.GetParameters()[0].ParameterType.GenericTypeArguments.Contains(messageType) || m.GetParameters()[0]
                   .ParameterType.GenericTypeArguments.First().GetTypeInfo()
                   .IsAssignableFrom(messageType.GetTypeInfo()));
    }

    private static Type GetHandleMethodContextType(MethodBase m, Type messageType)
    {
        var parameterType = m.GetParameters()[0].ParameterType;

        if (typeof(ICommandContext<>).MakeGenericType(messageType) == parameterType)
            return typeof(CommandContext<>).MakeGenericType(messageType);
        if (typeof(IRequestContext<>).MakeGenericType(messageType) == parameterType)
            return typeof(RequestContext<>).MakeGenericType(messageType);
        if (typeof(IEventContext<>).MakeGenericType(messageType) == parameterType)
            return typeof(EventContext<>).MakeGenericType(messageType);
        return typeof(MessageContext<>).MakeGenericType(messageType);
    }
    
    private static object GetResultFromTask(Task task)
    {
        if (!task.GetType().GetTypeInfo().IsGenericType)
            return null;
        var result = task.GetType().GetRuntimeProperty("Result").GetMethod;
        return result.Invoke(task, new object[] { });
    }
}