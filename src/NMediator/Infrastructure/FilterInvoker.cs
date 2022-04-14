using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;

namespace NMediator.Infrastructure;

public class FilterInvoker
{
    private const string FilterOnExecutingMethod = "OnExecuting";
    private const string FilterOnExecutedMethod = "OnExecuted";
    
    private readonly IMessageContext<IMessage> _context;
    
    public FilterInvoker(IMessageContext<IMessage> context)
    {
        _context = context;
    }
    
    public async Task InvokeExecutionFilter(Type filterType, Func<Task> continuation, CancellationToken cancellationToken)
    {
        var filter = _context.Scope.Resolve(filterType);

        await InvokeFilterMethod(filter, FilterOnExecutingMethod, _context, cancellationToken).ConfigureAwait(false);
        
        var wasError = false;
        try
        {
            await continuation().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            wasError = true;
            _context.Exception = ex is TargetInvocationException ? ex.InnerException : ex;
            
            await InvokeFilterMethod(filter, FilterOnExecutedMethod, _context, cancellationToken).ConfigureAwait(false);
            
            if (!_context.ExceptionHandled)
                throw;
        }

        if (!wasError)
            await InvokeFilterMethod(filter, FilterOnExecutedMethod, _context, cancellationToken).ConfigureAwait(false);
    }

    public async Task InvokeExceptionFilter(Type exceptionFilterType, CancellationToken cancellationToken)
    {
        if (_context.ExceptionHandled) return;
        
        var filter = (IExceptionFilter)_context.Scope.Resolve(exceptionFilterType);

        await filter.OnException(_context, cancellationToken).ConfigureAwait(false);
    }
    
    private static async Task InvokeFilterMethod(object filter, string method, IMessageContext<IMessage> context, CancellationToken cancellationToken)
    {
        var executeMethod = GetFilterMethod(filter, method, context.Message.GetType());

        var handleTask = (Task)executeMethod.Invoke(filter, new object[] { context, cancellationToken });
            
        if (handleTask == null) return;

        await handleTask.ConfigureAwait(false);
    }

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