using Microsoft.Extensions.DependencyInjection;
using NMediator.Ioc;

namespace NMediator.Extensions.Microsoft.DependencyInjection;

public class MicrosoftDependencyScope : IDependencyScope
{
    private readonly IServiceScope _serviceScope;
    
    public MicrosoftDependencyScope(IServiceScope serviceScope)
    {
        _serviceScope = serviceScope;
    }

    public T Resolve<T>()
    {
        return _serviceScope.ServiceProvider.GetService<T>();
    }

    public object Resolve(Type serviceType)
    {
        return _serviceScope.ServiceProvider.GetService(serviceType);
    }

    public IDependencyScope BeginScope()
    {
        return new MicrosoftDependencyScope(_serviceScope.ServiceProvider.CreateScope());
    }
    
    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}