using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NMediator.Ioc;
using NMediator.Middleware;

namespace NMediator
{
    public class MediatorConfiguration
    {
        private IDependencyScope _resolver;
        
        public readonly List<MiddlewareProcessor> MiddlewareProcessors = new();

        public readonly ConcurrentDictionary<Type, List<Type>> MessageHandlerBindings = new();

        public MediatorConfiguration()
        {
            _resolver = new DefaultDependencyScope();
        }

        public MediatorConfiguration UseDependencyScope(IDependencyScope scope)
        {
            _resolver = scope;
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

        public MediatorConfiguration UseMiddleware<TMiddleware>()
            where TMiddleware : class, IMiddleware
        {
            return this.RegisterMiddleware<TMiddleware>();
        }

        private MiddlewareProcessor BuildPipeline()
        {
            return MiddlewareProcessors.First();
        }
        
        public IMediator CreateMediator()
        {
            UseMiddleware<HandlerInvokerMiddleware>();
            
            var pipeline = BuildPipeline();
            
            return new Mediator(_resolver, pipeline, MessageHandlerBindings);
        }
    }
}