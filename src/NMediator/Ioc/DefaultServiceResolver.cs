using System;
using System.Collections.Generic;

namespace NMediator.Ioc
{
    public class DefaultServiceResolver : IServiceResolver
    {
        private readonly Dictionary<Type, List<Type>> _registrations;

        public DefaultServiceResolver(Dictionary<Type, List<Type>> registrations)
        {
            _registrations = registrations;
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