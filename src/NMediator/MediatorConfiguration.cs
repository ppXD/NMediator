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
        private IServiceResolver _serviceResolver;
        
        private readonly IList<Func<MessageDelegate, MessageDelegate>> _middlewares = new List<Func<MessageDelegate, MessageDelegate>>();

        public IServiceResolver Resolver => _serviceResolver ?? (_serviceResolver = _serviceRegistration.CreateResolver());

        public readonly List<Type> MiddlewareTypes = new List<Type>();
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

        public MediatorConfiguration RegisterServices(Action<IServiceRegistration> register)
        {
            register(_serviceRegistration);
            
            return this;
        }
        
        public MediatorConfiguration RegisterHandlers(params Assembly[] assemblies)
        {
            return this.RegisterHandlers(assemblies.SelectMany(assembly => assembly.GetTypes()));
        }
        
        public MediatorConfiguration UseMiddleware(Func<MessageDelegate, MessageDelegate> middleware)
        {
            _middlewares.Add(middleware);
            
            return this;
        }

        public MediatorConfiguration UseMiddleware<TMiddleware>()
            where TMiddleware : IMiddleware
        {
            var middlewareType = typeof(TMiddleware);
            
            if (!MiddlewareTypes.Contains(middlewareType))
            {
                MiddlewareTypes.Add(middlewareType);
            
                _serviceRegistration.Register(middlewareType);
            }
            
            return UseMiddleware(next =>
            {
                return async message =>
                {
                    await ((IMiddleware) Resolver.Resolve(middlewareType)).InvokeAsync(message, next);
                };
            });
        }
        
        internal MessageDelegate BuildPipeline()
        {
            Task Seed(object message) => Task.CompletedTask;

            return _middlewares.Reverse().Aggregate((MessageDelegate) Seed, (next, current) => current(next));
        }
        
        public IMediator CreateMediator()
        {
            if (_mediatorCreated)  
                throw new InvalidOperationException("CreateMediator() was previously called and can only be called once.");

            _mediatorCreated = true;
            
            return new Mediator(this);
        }
    }
}