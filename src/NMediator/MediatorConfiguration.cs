using System;
using System.Linq;
using System.Reflection;
using NMediator.Filters;
using NMediator.Internal;

namespace NMediator;

public class MediatorConfiguration
{
    private bool _mediatorCreated;

    private IDependencyScope _resolver;
    
    public IDependencyScope Resolver => _resolver ??= new DefaultDependencyScope();

    public MediatorFilterConfiguration FilterConfiguration { get; } = new();

    public MediatorHandlerConfiguration HandlerConfiguration { get; } = new();
    
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
    
    public MediatorConfiguration UseFilter<TFilter>()
        where TFilter : class, IFilter
    {
        FilterConfiguration.UseFilter<TFilter>();
        return this;
    }

    public MediatorConfiguration UseFilter(Type filter)
    {
        FilterConfiguration.UseFilter(filter);
        return this;
    }
    
    public MediatorConfiguration UseFilter(IFilter filter)
    {
        FilterConfiguration.UseFilter(filter);
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