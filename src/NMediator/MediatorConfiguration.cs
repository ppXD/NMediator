using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using NMediator.Ioc;

namespace NMediator
{
    public class MediatorConfiguration
    {
        private bool _mediatorCreated;

        private IServiceRegistration _serviceRegistration;
        
        private readonly IList<Func<MessageDelegate, MessageDelegate>> _middlewares = new List<Func<MessageDelegate, MessageDelegate>>();

        public IServiceResolver Resolver => _serviceRegistration?.CreateResolver();
        
        public MediatorConfiguration UseServiceRegistration(IServiceRegistration serviceRegistration)
        {
            _serviceRegistration = serviceRegistration;
            
            return this;
        }
        
        public MediatorConfiguration RegisterHandlers(params Assembly[] assemblies)
        {
            return this;
        }

        public MediatorConfiguration UseMiddleware(Func<MessageDelegate, MessageDelegate> middleware)
        {
            _middlewares.Add(middleware);
            
            return this;
        }
        
        public Mediator CreateMediator()
        {
            if (_mediatorCreated)  
                throw new InvalidOperationException("CreateMediator() was previously called and can only be called once.");

            _mediatorCreated = true;

            MessageDelegate seed = message => Task.CompletedTask;

            seed = _middlewares.Reverse().Aggregate(seed, (next, current) => current(next));

            return new Mediator(seed);
        }
    }
}