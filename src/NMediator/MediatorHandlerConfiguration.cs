using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMediator.Internal;

namespace NMediator;

public class MediatorHandlerConfiguration
{
    private ConcurrentDictionary<HandlerKey, List<HandlerWrapper>> HandlerMappings { get; } = new();

    public IEnumerable<Type> GetHandlers() =>
        HandlerMappings.SelectMany(x => x.Value).Select(x => x.Handler).Distinct().ToList();

    public IList<HandlerWrapper> GetHandlers(IMessage message, Type responseType)
    {
        var messageType = message.GetType();
    
        HandlerMappings.TryGetValue(new HandlerKey(messageType, responseType), out var handlers);
    
        if (handlers == null)
            throw new NoHandlerFoundException(messageType);
        
        return handlers.OrderBy(m => Prioritize(m, message, responseType)).ToList();
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
            .Where(x => x.IsClass && !x.IsAbstract)
            .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == targetHandlerType)).ToList();
        
        foreach (var handlerType in handlers)
        {
            foreach (var implementedInterface in handlerType.GetTypeInfo().ImplementedInterfaces.Where(i => IsHandlerInterface(i, targetHandlerType)))
            {
                var messageType = implementedInterface.GenericTypeArguments[0];
                var responseType = implementedInterface.GenericTypeArguments.Length > 1 ? implementedInterface.GenericTypeArguments[1] : null;
                
                var keys = new List<HandlerKey>(new[]
                {
                    new HandlerKey(messageType, responseType)
                });

                keys.AddRange(messageType.Assembly.GetTypes()
                    .Where(x => x.IsClass && messageType.IsAssignableFrom(x))
                    .Select(classType => new HandlerKey(classType, responseType)));
                
                if (responseType != null)
                {
                    var messageTypes = keys.Select(m => m.MessageType).ToList();
                    
                    keys.AddRange(responseType.Assembly.GetTypes()
                        .Where(type => responseType.IsSubclassOf(type))
                        .SelectMany(subClassType =>
                            messageTypes.Select(m => new HandlerKey(m, subClassType))));
                    
                    if (responseType.IsInterface)
                        keys.AddRange(responseType.Assembly.GetTypes()
                            .Where(type => responseType.IsAssignableFrom(type))
                            .SelectMany(implement =>
                                messageTypes.Select(m => new HandlerKey(m, implement))));
                }

                foreach (var key in keys)
                {
                    var handlerWrapper = new HandlerWrapper(handlerType, messageType, responseType);

                    if (HandlerMappings.ContainsKey(key))
                    {
                        if (!HandlerMappings[key].Contains(handlerWrapper))
                            HandlerMappings[key].Add(handlerWrapper);
                    }
                    else
                    {
                        HandlerMappings.TryAdd(key, new List<HandlerWrapper> {handlerWrapper});
                    }
                }
            }
        }
    }
    
    private static bool IsHandlerInterface(Type type, Type handleType)
    {
        return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == handleType;
    }

    private static int Prioritize(HandlerWrapper handler, IMessage message, Type responseType)
    {
        return message switch
        {
            not null when message.GetType() == handler.MessageType && 
                          responseType == handler.ResponseType => 1,
            not null when message.GetType() == handler.MessageType &&
                          responseType.IsAssignableFrom(handler.ResponseType) => 2,
            not null when message.GetType().IsSubclassOf(handler.MessageType) &&
                          responseType == handler.ResponseType => 3,
            not null when message.GetType().IsSubclassOf(handler.MessageType) &&
                          responseType.IsAssignableFrom(handler.ResponseType) => 4,
            _ => 5
        };
    }
    
    private class HandlerKey : IEquatable<HandlerKey>
    {
        public HandlerKey(Type messageType, Type responseType)
        {
            MessageType = messageType;
            ResponseType = responseType;
        }

        public bool Equals(HandlerKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return MessageType == other.MessageType && ResponseType == other.ResponseType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == GetType() && Equals((HandlerKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((MessageType != null ? MessageType.GetHashCode() : 0) * 397) ^ (ResponseType != null ? ResponseType.GetHashCode() : 0);
            }
        }

        public static bool operator ==(HandlerKey left, HandlerKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(HandlerKey left, HandlerKey right)
        {
            return !Equals(left, right);
        }

        public Type MessageType { get; }

        public Type ResponseType { get; }
    }
}