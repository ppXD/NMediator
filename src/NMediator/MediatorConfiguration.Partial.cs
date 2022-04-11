using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NMediator.Middlewares;

namespace NMediator;

public partial class MediatorConfiguration
{
    private void RegisterHandlersInternal(IEnumerable<Type> handlerTypes)
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
                
                if (MessageHandlerBindings.ContainsKey(messageType))
                {
                    if (!MessageHandlerBindings[messageType].Contains(handlerType))
                        MessageHandlerBindings[messageType].Add(handlerType);
                }
                else
                {
                    MessageHandlerBindings.TryAdd(messageType, new List<Type> {handlerType});
                }
            }
        }
    }

    private void RegisterMiddleware<TMiddleware>()
        where TMiddleware : class, IMiddleware
    {
        var processors = new MiddlewareProcessor(typeof(TMiddleware));

        if (MiddlewareProcessors.Any())
            MiddlewareProcessors.Last().Next = processors;
        MiddlewareProcessors.Add(processors);
    }
    
    private bool IsHandlerInterface(Type type, Type handleType)
    {
        return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == handleType;
    }
}
