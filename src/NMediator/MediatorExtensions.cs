using System;
using System.Reflection;

namespace NMediator
{
    public static class MediatorExtensions
    {
        public static MediatorConfiguration UseMiddleware<TMiddleware>(this MediatorConfiguration mediatorConfiguration)
            where TMiddleware : IMiddleware
        {
            return UseMiddleware(mediatorConfiguration, typeof(TMiddleware));
        }

        private static MediatorConfiguration UseMiddleware(this MediatorConfiguration mediatorConfiguration, Type middlewareType)
        {
            if (typeof(IMiddleware).GetTypeInfo().IsAssignableFrom(middlewareType.GetTypeInfo()))
            {
                return UseMiddlewareInterface(mediatorConfiguration, middlewareType);
            }

            return null;
        }

        private static MediatorConfiguration UseMiddlewareInterface(MediatorConfiguration mediatorConfiguration, Type middlewareType)
        {
            return mediatorConfiguration.UseMiddleware(next =>
            {
                return async message =>
                {
                    var middleware = mediatorConfiguration.Resolver == null
                        ? Activator.CreateInstance(middlewareType)
                        : mediatorConfiguration.Resolver.Resolve(middlewareType);

                    await ((IMiddleware) middleware).InvokeAsync(message, next);
                };
            });
        }
    }
}