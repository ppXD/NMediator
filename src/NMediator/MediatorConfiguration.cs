using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NMediator.Ioc;
using NMediator.Middlewares;

namespace NMediator;

public partial class MediatorConfiguration
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
        return RegisterHandlers(handlerType);
    }

    public MediatorConfiguration RegisterHandler<THandler>()
    {
        return RegisterHandlers(typeof(THandler));
    }

    public MediatorConfiguration RegisterHandlers(params Assembly[] assemblies)
    {
        return RegisterHandlers(assemblies.SelectMany(assembly => assembly.GetTypes()).ToArray());
    }
    
    public MediatorConfiguration RegisterHandlers(params Type[] handlerTypes)
    {
        RegisterHandlersInternal(handlerTypes);
        return this;
    }

    public MediatorConfiguration UseMiddleware<TMiddleware>()
        where TMiddleware : class, IMiddleware
    {
        RegisterMiddleware<TMiddleware>();
        return this;
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