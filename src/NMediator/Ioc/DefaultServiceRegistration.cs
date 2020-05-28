using System;
using System.Collections.Generic;

namespace NMediator.Ioc
{
    public class DefaultServiceRegistration : IServiceRegistration
    {
        private readonly Dictionary<Type, List<Type>> _registrations = new Dictionary<Type, List<Type>>();
        
        public void Register<TService, TImplementation>(Lifetime lifetime = Lifetime.AlwaysUnique) 
            where TService : class 
            where TImplementation : class, TService
        {
            Register(typeof(TService), typeof(TImplementation));
        }

        public void Register(Type serviceType, Type implementationType, Lifetime lifetime = Lifetime.AlwaysUnique)
        {
            if (_registrations.ContainsKey(serviceType))
            {
                _registrations[serviceType].Add(implementationType);
            }
            else
            {
                _registrations.Add(serviceType, new List<Type> {implementationType});
            }
        }

        public void Register(Type serviceType, Lifetime lifetime = Lifetime.AlwaysUnique)
        {
            
        }

        public IServiceResolver CreateResolver()
        {
            return new DefaultServiceResolver(_registrations);
        }
    }
}