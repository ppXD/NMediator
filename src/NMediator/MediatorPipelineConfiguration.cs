using System;
using System.Linq;
using System.Collections.Generic;
using NMediator.Filters;
using NMediator.Middlewares;

namespace NMediator;

public class MediatorPipelineConfiguration
{
    public List<Type> Filters { get; } = new();
    public List<Type> Middlewares { get; } = new();

    private MiddlewareProcessor _pipelineProcessor;

    public MiddlewareProcessor PipelineProcessor => _pipelineProcessor ??= BuildPipeline();
    
    protected internal void UseFilter(Type filter)
    {
        if (!typeof(IFilter).IsAssignableFrom(filter)) return;
        {
            Filters.Add(filter);
        }
    }
    
    protected internal void UseMiddleware(Type middleware)
    {
        if (!typeof(IMiddleware).IsAssignableFrom(middleware)) return;
        {
            Middlewares.Add(middleware);
        }
    }
    
    protected internal IEnumerable<Type> FindFilters<TMessage>() where TMessage : class, IMessage
    {
        var messageType = typeof(TMessage);

        var matchedFilterTypes = new List<Type>
        {
            typeof(IMessageFilter), typeof(IMessageFilter<TMessage>), typeof(IExceptionFilter)
        };
        
        switch (messageType)
        {
            case not null when typeof(ICommand).IsAssignableFrom(messageType):
                matchedFilterTypes.AddRange(new[] { typeof(ICommandFilter), typeof(ICommandFilter<>).MakeGenericType(messageType) });
                break;
            case not null when typeof(IRequest).IsAssignableFrom(messageType):
                matchedFilterTypes.AddRange(new[] { typeof(IRequestFilter), typeof(IRequestFilter<>).MakeGenericType(messageType) });
                break;
            case not null when typeof(IEvent).IsAssignableFrom(messageType):
                matchedFilterTypes.AddRange(new[] { typeof(IEventFilter), typeof(IEventFilter<>).MakeGenericType(messageType) });
                break;
        }
        
        var filters = Filters.Where(f =>
            f.GetInterfaces().Any(i => matchedFilterTypes.Any(m => i == m || i.IsGenericType && i.GetGenericTypeDefinition() == m)));

        return filters;
    }
    
    private MiddlewareProcessor BuildPipeline()
    {
        OrderMiddlewares();
        
        MiddlewareProcessor processor = null;
        
        for (var i = Middlewares.Count - 1; i >= 0; i--)
        {
            processor = i == Middlewares.Count - 1
                ? new MiddlewareProcessor(Middlewares[i], null)
                : new MiddlewareProcessor(Middlewares[i], processor);
        }
        
        return processor;
    }
    
    private void OrderMiddlewares()
    {
        Middlewares.Remove(Middlewares.Single(x => x == typeof(InvokeFilterPipelineMiddleware)));
        Middlewares.Add(typeof(InvokeFilterPipelineMiddleware));
    }
}