using System;
using System.Collections.Generic;

namespace NMediator.Ioc
{
    public class DefaultServiceResolver : IServiceResolver
    {
        private readonly List<Type> _middlewareRegistrations;
        private readonly Dictionary<Type, List<Type>> _handlerRegistrations;

        public DefaultServiceResolver(List<Type> middlewareRegistrations, Dictionary<Type, List<Type>> handlerRegistrations)
        {
            _middlewareRegistrations = middlewareRegistrations;
            _handlerRegistrations = handlerRegistrations;
        }

        public T Resolve<T>()
        {
            return (T) Resolve(typeof(T));
        }

        public object Resolve(Type serviceType)
        {
            return Activator.CreateInstance(serviceType);
        }

        public void Dispose()
        {
        }
    }
}