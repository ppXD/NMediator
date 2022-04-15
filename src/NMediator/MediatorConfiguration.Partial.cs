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
        if (handlerTypes == null || !handlerTypes.Any()) return;
        
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

    private void RegisterMiddlewareInternal(Type middleware)
    {
        if (!typeof(IMiddleware).IsAssignableFrom(middleware)) return;
        {
            Middlewares.Add(middleware);
            MoveInvokeFilterPipelineMiddlewareToBottom();
        }
    }

    private void RegisterFiltersInternal(IEnumerable<Type> filterTypes)
    {
        var filters = filterTypes
            .Where(x => !x.IsAbstract)
            .Where(x => x.IsClass && typeof(IFilter).IsAssignableFrom(x)).ToList();

        Filters.AddRange(filters);
    }
    
    private bool IsHandlerInterface(Type type, Type handleType)
    {
        return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == handleType;
    }

    private void MoveInvokeFilterPipelineMiddlewareToBottom()
    {
        Middlewares.Remove(Middlewares.Single(x => x == typeof(InvokeFilterPipelineMiddleware)));
        Middlewares.Add(typeof(InvokeFilterPipelineMiddleware));
    }
}
