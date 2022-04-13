using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator.Infrastructure;

public class HandlerInvoker
{
    private readonly IMessageContext<IMessage> _context;
    
    public HandlerInvoker(IMessageContext<IMessage> context)
    {
        _context = context;
    }
    
    public async Task<object> Invoke(CancellationToken cancellationToken)
    {
        if (_context.MessageBindingHandlers == null || !_context.MessageBindingHandlers.Any())
            throw new NoHandlerFoundException(_context.Message.GetType());

        if (_context.Message is ICommand or IRequest)
        {
            if (_context.MessageBindingHandlers.Count() > 1)
                throw new MoreThanOneHandlerException(_context.Message.GetType());

            return await InvokeHandleMethod(_context.MessageBindingHandlers.Single(), _context, cancellationToken)
                .ConfigureAwait(false);
        }

        foreach (var handlerType in _context.MessageBindingHandlers)
        {
            await InvokeHandleMethod(handlerType, _context, cancellationToken, false)
                .ConfigureAwait(false);
        }

        return null;
    }

    private static async Task<object> InvokeHandleMethod(Type handlerType, IMessageContext<IMessage> context, CancellationToken cancellationToken, bool shouldGetResult = true)
    {
        var handler = context.Scope.Resolve(handlerType);
        var handleMethod = GetHandleMethod(handlerType, context.Message.GetType(), context.ResponseType);

        var handleTask = (Task)handleMethod.Invoke(handler, new object[] { context, cancellationToken });

        if (handleTask == null) return null;

        await handleTask.ConfigureAwait(false);

        return shouldGetResult ? GetResultFromTask(handleTask) : null;
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

    private static object GetResultFromTask(Task task)
    {
        if (!task.GetType().GetTypeInfo().IsGenericType)
            return null;
        var result = task.GetType().GetRuntimeProperty("Result")?.GetMethod;
        return result?.Invoke(task, Array.Empty<object>());
    }
}