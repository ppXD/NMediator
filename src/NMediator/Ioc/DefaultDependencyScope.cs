using System;

namespace NMediator.Ioc;

public class DefaultDependencyScope : IDependencyScope
{
    public T Resolve<T>()
    {
        return (T) Resolve(typeof(T));
    }

    public object Resolve(Type serviceType)
    {
        return Activator.CreateInstance(serviceType);
    }

    public IDependencyScope BeginScope()
    {
        return new DefaultDependencyScope();
    }

    public void Dispose()
    {
    }
}