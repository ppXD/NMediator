using System;
using System.Linq;
using System.Reflection;
using NMediator.Ioc;
using NMediator.Filters;
using NMediator.Middlewares;

namespace NMediator;

public class MediatorConfiguration
{
    private bool _mediatorCreated;

    private IDependencyScope _resolver;
    
    public IDependencyScope Resolver => _resolver ??= new DefaultDependencyScope();

    public MediatorHandlerConfiguration HandlerConfiguration { get; } = new();

    public MediatorPipelineConfiguration PipelineConfiguration { get; } = new();

    public MediatorConfiguration()
    {
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
        HandlerConfiguration.RegisterHandlers(handlerTypes);
        return this;
    }

    public MediatorConfiguration UseMiddleware<TMiddleware>()
        where TMiddleware : class, IMiddleware
    {
        return UseMiddleware(typeof(TMiddleware));
    }

    public MediatorConfiguration UseMiddleware(Type middleware)
    {
        PipelineConfiguration.UseMiddleware(middleware);
        return this;
    }
    
    public MediatorConfiguration UseFilter<TFilter>()
        where TFilter : class, IFilter
    {
        return UseFilter(typeof(TFilter));
    }

    public MediatorConfiguration UseFilter(Type filter)
    {
        PipelineConfiguration.UseFilter(filter);
        return this;
    }
    
    public Mediator CreateMediator()
    {
        if (_mediatorCreated)  
            throw new InvalidOperationException("CreateMediator() was previously called and can only be called once.");

        _mediatorCreated = true;
        
        return new Mediator(this);
    }
}