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
        if (_context.Handlers == null || !_context.Handlers.Any())
            throw new NoHandlerFoundException(_context.Message.GetType());

        if (_context.Message is not IEvent)
        {
            if (_context.Handlers.Count() > 1)
                throw new MoreThanOneHandlerException(_context.Message.GetType());

            return await InvokeHandleMethod(_context.Handlers.Single(), _context, cancellationToken)
                .ConfigureAwait(false);
        }

        foreach (var handler in _context.Handlers)
        {
            await InvokeHandleMethod(handler, _context, cancellationToken, false)
                .ConfigureAwait(false);
        }

        return null;
    }

    private static async Task<object> InvokeHandleMethod(HandlerWrapper handlerWrapper, IMessageContext<IMessage> context, CancellationToken cancellationToken, bool shouldGetResult = true)
    {
        var handler = context.Scope.Resolve(handlerWrapper.Handler);
        var handleMethod = GetHandleMethod(handlerWrapper.Handler, context.Message.GetType(), handlerWrapper.ResponseType);

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