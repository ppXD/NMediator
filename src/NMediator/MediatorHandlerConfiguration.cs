using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NMediator;

public class MediatorHandlerConfiguration
{
    private ConcurrentDictionary<Type, List<Type>> HandlerMappings { get; } = new();

    public IEnumerable<Type> GetHandlers() => HandlerMappings.SelectMany(x => x.Value).Distinct().ToList();

    public IEnumerable<Type> GetHandlers(IMessage message, IEnumerable<Type> typesToMatch)
    {
        var messageType = message.GetType();
        
        HandlerMappings.TryGetValue(messageType, out var handlerTypes);

        if (handlerTypes == null)
            throw new NoHandlerFoundException(messageType);

        if (typeof(ICommand).IsAssignableFrom(messageType) && handlerTypes.Count > 1)
            throw new MoreThanOneHandlerException(messageType);
        
        handlerTypes = handlerTypes.Where(handlerType =>
                handlerType.GetInterfaces()
                    .Any(i => typesToMatch.Any(m => i == m || i.GetGenericTypeDefinition() == m)))
            .ToList();
        
        if (!handlerTypes.Any())
            throw new NoHandlerFoundException(messageType);
        
        return handlerTypes;
    }
    
    protected internal void RegisterHandlers(IReadOnlyCollection<Type> handlerTypes)
    {
        if (handlerTypes == null || !handlerTypes.Any()) return;
        
        RegisterHandlersInternal(handlerTypes, typeof(ICommandHandler<>));
        RegisterHandlersInternal(handlerTypes, typeof(ICommandHandler<,>));
        RegisterHandlersInternal(handlerTypes, typeof(IRequestHandler<,>));
        RegisterHandlersInternal(handlerTypes, typeof(IEventHandler<>));
    }

    private void RegisterHandlersInternal(IEnumerable<Type> handlerTypes, Type targetHandlerType)
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
                
                if (HandlerMappings.ContainsKey(messageType))
                {
                    if (!HandlerMappings[messageType].Contains(handlerType))
                        HandlerMappings[messageType].Add(handlerType);
                }
                else
                {
                    HandlerMappings.TryAdd(messageType, new List<Type> {handlerType});
                }
            }
        }
    }
    
    private static bool IsHandlerInterface(Type type, Type handleType)
    {
        return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == handleType;
    }
}