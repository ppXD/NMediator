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

    private readonly ConcurrentDictionary<Type, List<Type>> _messageHandlerBindings = new();

    public List<Type> Filters { get; } = new();
    public List<Type> Middlewares { get; } = new();
    public List<Type> Handlers => _messageHandlerBindings.SelectMany(x => x.Value).ToList();

    public MediatorConfiguration()
    {
        _resolver = new DefaultDependencyScope();

        UseMiddleware<InvokeFilterPipelineMiddleware>();
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
        return UseMiddleware(typeof(TMiddleware));
    }

    public MediatorConfiguration UseMiddleware(Type middleware)
    {
        RegisterMiddlewareInternal(middleware);
        return this;
    }
    
    public MediatorConfiguration UseFilter<TFilter>()
        where TFilter : class, IFilter
    {
        return UseFilters(typeof(TFilter));
    }

    public MediatorConfiguration UseFilter(Type filter)
    {
        return UseFilters(filter);
    }
    
    public MediatorConfiguration UseFilters(params Assembly[] assemblies)
    {
        return UseFilters(assemblies.SelectMany(assembly => assembly.GetTypes()).ToArray());
    }
    
    public MediatorConfiguration UseFilters(params Type[] filters)
    {
        RegisterFiltersInternal(filters);
        return this;
    }
    
    private MiddlewareProcessor BuildPipeline()
    {
        MiddlewareProcessor processor = null;
        
        for (var i = Middlewares.Count - 1; i >= 0; i--)
        {
            processor = i == Middlewares.Count - 1
                ? new MiddlewareProcessor(Middlewares[i], null)
                : new MiddlewareProcessor(Middlewares[i], processor);
        }
        
        return processor;
    }
    
    public IMediator CreateMediator()
    {
        return new Mediator(_resolver, BuildPipeline(), Filters, _messageHandlerBindings);
    }
}