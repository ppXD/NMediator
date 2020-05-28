using System;

namespace NMediator.Ioc
{
    public interface IServiceRegistration
    {
        void Register<TService, TImplementation>(
            Lifetime lifetime = Lifetime.AlwaysUnique)
            where TImplementation : class, TService
            where TService : class;
        
        void Register(
            Type serviceType,
            Type implementationType,
            Lifetime lifetime = Lifetime.AlwaysUnique);
        
        void Register(
            Type serviceType,
            Lifetime lifetime = Lifetime.AlwaysUnique);
        
        IServiceResolver CreateResolver();
    }
}