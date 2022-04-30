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
        
        return handlers.OrderBy(m => Prioritize(m, message, responseType));
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

                messageAndResponses.AddRange(messageType.Assembly.GetTypes()
                    .Where(x => x.IsClass && messageType.IsAssignableFrom(x))
                    .Select(classType => new MessageAndResponse(classType, responseType)));
                
                if (responseType != null)
                {
                    var messageTypes = messageAndResponses.Select(m => m.MessageType).ToList();
                    
                    messageAndResponses.AddRange(responseType.Assembly.GetTypes()
                        .Where(type => responseType.IsSubclassOf(type))
                        .SelectMany(subClassType =>
                            messageTypes.Select(m => new MessageAndResponse(m, subClassType))));
                }

                foreach (var messageAndResponse in messageAndResponses)
                {
                    var handlerWrapper = new HandlerWrapper(handlerType, messageType, responseType);

                    if (HandlerDispatchers.ContainsKey(messageAndResponse))
                    {
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

    private static int Prioritize(HandlerWrapper handler, IMessage message, Type responseType)
    {
        if (handler.MessageType == message.GetType() && handler.ResponseType == responseType)
            return 0;
        if (message.GetType().IsSubclassOf(handler.MessageType) && handler.ResponseType == responseType)
            return 1;
        if (message.GetType() == handler.MessageType)
            return 2;
        return 3;
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

        public Type MessageType { get; }

        public Type ResponseType { get; }
    }
}