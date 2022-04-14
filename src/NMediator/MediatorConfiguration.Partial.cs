using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NMediator.Filters;
using NMediator.Middlewares;

namespace NMediator;

public partial class MediatorConfiguration
{
    private void RegisterHandlersInternal(IReadOnlyCollection<Type> handlerTypes)
    {
        RegisterHandlers(handlerTypes, typeof(ICommandHandler<>));
        RegisterHandlers(handlerTypes, typeof(ICommandHandler<,>));
        RegisterHandlers(handlerTypes, typeof(IRequestHandler<,>));
        RegisterHandlers(handlerTypes, typeof(IEventHandler<>));
    }

    private void RegisterHandlers(IEnumerable<Type> handlerTypes, Type targetHandlerType)
    {
        var handlers = handlerTypes
            .Where(x => !x.IsAbstract)
            .Where(x => x.IsClass && x.GetInterfaces()
            .Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == targetHandlerType)).ToList();
        
        foreach (var handlerType in handlers)
        {
            foreach (var implementedInterface in handlerType.GetTypeInfo().ImplementedInterfaces)
            {
                if (!IsHandlerInterface(implementedInterface, targetHandlerType)) continue;

                var messageType = implementedInterface.GenericTypeArguments[0];
                
                if (_messageHandlerBindings.ContainsKey(messageType))
                {
                    if (!_messageHandlerBindings[messageType].Contains(handlerType))
                        _messageHandlerBindings[messageType].Add(handlerType);
                }
                else
                {
                    _messageHandlerBindings.TryAdd(messageType, new List<Type> {handlerType});
                }
            }
        }
    }

    private void RegisterMiddlewaresInternal(params Type[] middlewares)
    {
        foreach (var middleware in middlewares)
        {
            RegisterMiddleware(middleware);
        }
    }

    private void RegisterMiddleware(Type middleware)
    {
        if (!typeof(IMiddleware).IsAssignableFrom(middleware))
            throw new NotSupportedException(nameof(middleware));
        
        var processors = new MiddlewareProcessor(middleware);

        if (_middlewareProcessors.Any())
            _middlewareProcessors.Last().Next = processors;
        _middlewareProcessors.Add(processors);
    }

    private void RegisterFiltersInternal(params Type[] filters)
    {
        foreach (var filter in filters)
        {
            RegisterFilter(filter);
        }
    }

    private void RegisterFilter(Type filter)
    {
        if (!typeof(IFilter).IsAssignableFrom(filter))
            throw new NotSupportedException(nameof(filter));
        
        _filters.Add(filter);
    }
    
    private bool IsHandlerInterface(Type type, Type handleType)
    {
        return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == handleType;
    }
}
