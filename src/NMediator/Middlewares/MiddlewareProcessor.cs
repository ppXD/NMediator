using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Ioc;

namespace NMediator.Middlewares;

public class MiddlewareProcessor
{
    private readonly Type _middlewareType;

    public MiddlewareProcessor(Type middlewareType)
    {
        _middlewareType = middlewareType;
    }

    public async Task Process(IMessageContext<IMessage> context, CancellationToken cancellationToken)
    {
        if (context.Scope == null)
            throw new ArgumentNullException(nameof(IDependencyScope));

        var current = (IMiddleware)context.Scope.Resolve(_middlewareType);

        await current.OnExecuting(context, cancellationToken).ConfigureAwait(false);
        
        if (Next != null)
            await Next.Process(context, cancellationToken).ConfigureAwait(false);
            
        await current.OnExecuted(context, cancellationToken).ConfigureAwait(false);
    }

    public MiddlewareProcessor Next { get; set; }
}