using System;
using System.Collections.Generic;
using NMediator.Infrastructure;
using NMediator.Ioc;

namespace NMediator.Context;

public class EventContext<TEvent> : MessageContext<TEvent>, IEventContext<TEvent> where TEvent : IEvent
{
    public EventContext(IMessageContext<TEvent> context) : base(context.Message, context.Scope, context.ResponseType, context.Filters, context.Handlers)
    {
    }
    
    public EventContext(TEvent message, IDependencyScope scope, Type responseType = null, IEnumerable<Type> filters = null, IEnumerable<HandlerWrapper> handlers = null) : base(message, scope, responseType, filters, handlers)
    {
    }
}