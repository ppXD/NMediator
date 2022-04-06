using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Ioc;

namespace NMediator.Middleware
{
    public class MiddlewareProcessor
    {
        private readonly Type _middlewareType;

        public MiddlewareProcessor(Type middlewareType)
        {
            _middlewareType = middlewareType;
        }

        public async Task Process(IDependencyScope scope, CancellationToken cancellationToken)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var current = (IMiddleware)scope.Resolve(_middlewareType);

            await current.OnExecuting(null, cancellationToken).ConfigureAwait(false);
            
            if (Next != null)
                await Next.Process(scope, cancellationToken).ConfigureAwait(false);
            
            await current.OnExecuted(null, cancellationToken).ConfigureAwait(false);
        }

        public MiddlewareProcessor? Next { get; set; }
    }
}