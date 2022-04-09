using System;
using System.Collections.Generic;
using NMediator.Ioc;

namespace NMediator.Context;

public class EventContext<TEvent> : MessageContext<TEvent>, IEventContext<TEvent> where TEvent : IMessage
{
    public EventContext(IMessageContext<TEvent> context) : base(context.Message, context.Scope, context.ResponseType, context.MessageBindingHandlers)
    {
    }
    
    public EventContext(TEvent message, IDependencyScope scope, Type responseType = null, IEnumerable<Type> messageBindingHandlers = null) : base(message, scope, responseType, messageBindingHandlers)
    {
    }
}