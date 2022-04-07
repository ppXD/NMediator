using System;
using System.Collections.Generic;
using System.Linq;

namespace NMediator
{
    public partial class Mediator
    {
        private List<Type> FindHandlerTypes<TMessage>(IEnumerable<Type> matchedHandlerTypes) where TMessage : IMessage
        {
            var messageType = typeof(TMessage);
            
            _messageHandlerBindings.TryGetValue(messageType, out var handlerTypes);

            if (handlerTypes == null)
                throw new NoHandlerFoundException(messageType);

            handlerTypes = handlerTypes.Where(handlerType =>
                    handlerType.GetInterfaces()
                        .Any(i => matchedHandlerTypes.Any(m => i == m || i.GetGenericTypeDefinition() == m)))
                .ToList();
            
            if (!handlerTypes.Any())
                throw new NoHandlerFoundException(messageType);
            
            return handlerTypes;
        }
    }
}