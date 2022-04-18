using Autofac;
using NMediator.Ioc;

namespace NMediator.Extensions.Autofac;

public class AutofacDependencyScope : IDependencyScope
{
    private readonly ILifetimeScope _lifetimeScope;

    public AutofacDependencyScope(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    public T Resolve<T>()
    {
        return _lifetimeScope.Resolve<T>();
    }

    public object Resolve(Type serviceType)
    {
        return _lifetimeScope.Resolve(serviceType);
    }

    public IDependencyScope BeginScope()
    {
        return new AutofacDependencyScope(_lifetimeScope.BeginLifetimeScope());
    }

    public void Dispose() => _lifetimeScope.Dispose();
}