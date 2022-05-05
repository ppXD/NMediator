using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Internal;

public class FilterInvoker<TMessage> where TMessage : class, IMessage
{
    private const string FilterOnExecutingMethod = nameof(IHandlerFilter<TMessage>.OnHandlerExecuting);
    private const string FilterOnExecutedMethod = nameof(IHandlerFilter<TMessage>.OnHandlerExecuted);
    
    public async Task<HandlerExecutedContext<TMessage>> InvokeHandlerFilter(
        dynamic filter, HandlerExecutingContext<TMessage> preContext, 
        Func<Task<HandlerExecutedContext<TMessage>>> continuation, CancellationToken cancellationToken)
    {
        await filter.OnHandlerExecuting(preContext, cancellationToken).ConfigureAwait(false);
        
        if (preContext.Result != null)
        {
            return new HandlerExecutedContext<TMessage>(preContext.Message, preContext.Scope, preContext.Filters)
            {
                Result = preContext.Result
            };
        }
        
        var wasError = false;
        
        HandlerExecutedContext<TMessage> postContext;
        
        try
        {
            postContext = await continuation().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            wasError = true;
            postContext = new HandlerExecutedContext<TMessage>(preContext.Message, preContext.Scope, preContext.Filters);
            
            await filter.OnHandlerExecuted(postContext, cancellationToken).ConfigureAwait(false);
            
            if (!postContext.ExceptionHandled)
                throw;
        }

        if (!wasError)
            await filter.OnHandlerExecuted(postContext, cancellationToken).ConfigureAwait(false);

        return postContext;
    }

    public async Task InvokeExceptionFilter(Type exceptionFilterType, CancellationToken cancellationToken)
    {
        // if (_context.ExceptionHandled) return;
        //
        // var filter = (IExceptionFilter)_context.Scope.Resolve(exceptionFilterType);
        //
        // await filter.OnException(_context, cancellationToken).ConfigureAwait(false);
    }
    
    // private static async Task InvokeFilterMethod(object filter, string method, IMessageContext<IMessage> context, CancellationToken cancellationToken)
    // {
    //     var executeMethod = GetFilterMethod(filter, method, context.Message.GetType());
    //
    //     var handleTask = (Task)executeMethod.Invoke(filter, new object[] { context, cancellationToken });
    //         
    //     if (handleTask == null) return;
    //
    //     await handleTask.ConfigureAwait(false);
    // }

    private static MethodInfo GetFilterMethod(object filter, string method, Type messageType)
    {
        return filter.GetType().GetRuntimeMethods().Single(m => IsExecuteMethod(m, method, messageType));
    }

    private static bool IsExecuteMethod(MethodInfo m, string method, Type messageType)
    {
        return m.Name == method && m.IsPublic && ContainsReturnType(m) && ContainsParameter(m, messageType);
    }

    private static bool ContainsReturnType(MethodInfo m)
    {
        return m.ReturnType == typeof(Task);
    }

    private static bool ContainsParameter(MethodBase m, Type messageType)
    {
        return m.GetParameters().Any()
               && (m.GetParameters()[0].ParameterType.GenericTypeArguments.Contains(messageType) || m.GetParameters()[0]
                   .ParameterType.GenericTypeArguments.First().GetTypeInfo()
                   .IsAssignableFrom(messageType.GetTypeInfo()));
    }
}