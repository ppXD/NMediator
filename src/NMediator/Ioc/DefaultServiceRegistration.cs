using System;
using System.Collections.Generic;

namespace NMediator.Ioc
{
    public class DefaultServiceRegistration : IServiceRegistration
    {
        private readonly List<Type> _middlewareRegistrations = new List<Type>();
        private readonly Dictionary<Type, List<Type>> _handlerRegistrations = new Dictionary<Type, List<Type>>();
        
        public void Register<TService, TImplementation>(Lifetime lifetime = Lifetime.AlwaysUnique) 
            where TService : class 
            where TImplementation : class, TService
        {
            Register(typeof(TService), typeof(TImplementation));
        }

        public void Register(Type serviceType, Type implementationType, Lifetime lifetime = Lifetime.AlwaysUnique)
        {
            if (_handlerRegistrations.ContainsKey(serviceType))
            {
                _handlerRegistrations[serviceType].Add(implementationType);
            }
            else
            {
                _handlerRegistrations.Add(serviceType, new List<Type> {implementationType});
            }
        }

        public void Register(Type serviceType, Lifetime lifetime = Lifetime.AlwaysUnique)
        {
            _middlewareRegistrations.Add(serviceType);
        }

        public IServiceResolver CreateResolver()
        {
            return new DefaultServiceResolver();
        }
    }
}