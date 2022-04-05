using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NMediator.Ioc;

namespace NMediator
{
    public class MediatorConfiguration
    {
        public IDependencyScope Resolver { get; private set; }

        public readonly ConcurrentDictionary<Type, List<Type>> MessageBindings = new ConcurrentDictionary<Type, List<Type>>();
        
        public MediatorConfiguration()
        {
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
        
        public IMediator CreateMediator()
        {
            return new Mediator(this);
        }
    }
}