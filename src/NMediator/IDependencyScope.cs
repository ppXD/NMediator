using System;

namespace NMediator;

public interface IDependencyScope : IDisposable
{
    T Resolve<T>();
    object Resolve(Type serviceType);
    IDependencyScope BeginScope();
}