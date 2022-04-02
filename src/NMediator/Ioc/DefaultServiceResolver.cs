using System;
using System.Collections.Generic;

namespace NMediator.Ioc
{
    public class DefaultServiceResolver : IServiceResolver
    {
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