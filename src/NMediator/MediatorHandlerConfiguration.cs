using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMediator.Infrastructure;

namespace NMediator;

public class MediatorHandlerConfiguration
{
    private ConcurrentDictionary<MessageAndResponse, List<HandlerWrapper>> HandlerDispatchers { get; } = new();

    public IEnumerable<Type> GetHandlers() =>
        HandlerDispatchers.SelectMany(x => x.Value).Select(x => x.Handler).Distinct().ToList();

    public IEnumerable<HandlerWrapper> GetHandlers(IMessage message, Type responseType)
    {
        var messageType = message.GetType();
    
        HandlerDispatchers.TryGetValue(new MessageAndResponse(messageType, responseType), out var handlers);
    
        if (handlers == null)
            throw new NoHandlerFoundException(messageType);
        
        return handlers;
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
                
                var messageAndResponses = new List<MessageAndResponse>(new[]
                {
                    new MessageAndResponse(messageType, responseType)
                });

                if (responseType != null)
                {
                    messageAndResponses.AddRange(responseType.Assembly.GetTypes()
                        .Where(type => responseType.IsSubclassOf(type))
                        .Select(subClassType => new MessageAndResponse(messageType, subClassType)));
                }

                foreach (var messageAndResponse in messageAndResponses)
                {
                    var handlerWrapper = new HandlerWrapper(handlerType, responseType);

                    if (HandlerDispatchers.ContainsKey(messageAndResponse))
                    {
                        if (!typeof(IEvent).IsAssignableFrom(messageType)) continue;
                        
                        if (!HandlerDispatchers[messageAndResponse].Contains(handlerWrapper))
                            HandlerDispatchers[messageAndResponse].Add(handlerWrapper);
                    }
                    else
                    {
                        HandlerDispatchers.TryAdd(messageAndResponse, new List<HandlerWrapper> {handlerWrapper});
                    }
                }
            }
        }
    }
    
    private static bool IsHandlerInterface(Type type, Type handleType)
    {
        return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == handleType;
    }
    
    private class MessageAndResponse : IEquatable<MessageAndResponse>
    {
        public MessageAndResponse(Type messageType, Type responseType)
        {
            MessageType = messageType;
            ResponseType = responseType;
        }

        public bool Equals(MessageAndResponse other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return MessageType == other.MessageType && ResponseType == other.ResponseType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == GetType() && Equals((MessageAndResponse)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((MessageType != null ? MessageType.GetHashCode() : 0) * 397) ^ (ResponseType != null ? ResponseType.GetHashCode() : 0);
            }
        }

        public static bool operator ==(MessageAndResponse left, MessageAndResponse right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MessageAndResponse left, MessageAndResponse right)
        {
            return !Equals(left, right);
        }

        private Type MessageType { get; }

        private Type ResponseType { get; }
    }
}