using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Concurrent;
using NMediator.Ioc;
using NMediator.Filters;
using NMediator.Middlewares;

namespace NMediator;

public partial class MediatorConfiguration
{
    private IDependencyScope _resolver;

    private readonly List<IFilter> _filters = new();

    private readonly List<MiddlewareProcessor> _middlewareProcessors = new();

    private readonly ConcurrentDictionary<Type, List<Type>> _messageHandlerBindings = new();

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
        if (handlerTypes == null || !handlerTypes.Any())
            throw new ArgumentNullException(nameof(handlerTypes));
        RegisterHandlersInternal(handlerTypes);
        return this;
    }

    public MediatorConfiguration UseMiddleware<TMiddleware>()
        where TMiddleware : class, IMiddleware
    {
        UseMiddlewares(typeof(TMiddleware));
        return this;
    }

    public MediatorConfiguration UseMiddleware(Type middleware)
    {
        UseMiddlewares(middleware);
        return this;
    }
    
    public MediatorConfiguration UseMiddlewares(params Type[] middlewares)
    {
        RegisterMiddlewaresInternal(middlewares);
        return this;
    }
    
    public MediatorConfiguration UseFilter<TFilter>()
        where TFilter : class, IFilter
    {
        return this;
    }
    
    private MiddlewareProcessor BuildPipeline()
    {
        return _middlewareProcessors.First();
    }
    
    public IMediator CreateMediator()
    {
        UseMiddleware<HandlerInvokerMiddleware>();
            
        var pipeline = BuildPipeline();
            
        return new Mediator(_resolver, pipeline, _messageHandlerBindings);
    }
}