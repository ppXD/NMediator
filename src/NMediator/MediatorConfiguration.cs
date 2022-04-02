using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NMediator.Ioc;

namespace NMediator
{
    public class MediatorConfiguration
    {
        private IServiceRegistration _serviceRegistration;
        
        public IServiceResolver Resolver { get; private set; }

        public readonly Dictionary<Type, List<Type>> MessageBindings = new Dictionary<Type, List<Type>>();
        
        public MediatorConfiguration()
        {
            UseServiceRegistration(new DefaultServiceRegistration());
        }
        
        public MediatorConfiguration UseServiceRegistration(IServiceRegistration serviceRegistration)
        {
            _serviceRegistration = serviceRegistration;
            
            return this;
        }

        public MediatorConfiguration RegisterHandler(Type handlerType)
        {
            return this.RegisterHandlers(new[] {handlerType});
        }

        public MediatorConfiguration RegisterHandler<THandler>()
        {
            return this.RegisterHandlers(new[] {typeof(THandler)});
        }

        public MediatorConfiguration RegisterHandlers(params Assembly[] assemblies)
        {
            return this.RegisterHandlers(assemblies.SelectMany(assembly => assembly.GetTypes()));
        }
        
        public MediatorConfiguration RegisterServices(Action<IServiceRegistration> register)
        {
            register(_serviceRegistration);
            
            return this;
        }
        
        public IMediator CreateMediator()
        {
            Resolver = _serviceRegistration.CreateResolver();
            
            return new Mediator(this);
        }
    }
}