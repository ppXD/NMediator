using System;

namespace NMediator.Ioc
{
    public interface IDependencyScope : IDisposable
    {
        T Resolve<T>();
        object Resolve(Type serviceType);
        IDependencyScope BeginScope();
    }
}