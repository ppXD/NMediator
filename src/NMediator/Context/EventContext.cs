using System;
using System.Collections.Generic;
using NMediator.Infrastructure;

namespace NMediator.Context;

public class EventContext<TEvent> : MessageContext<TEvent>, IEventContext<TEvent> where TEvent : IEvent
{
    public EventContext(IMessageContext<TEvent> context) : base(context.Message, context.Scope, context.Filters, context.Handlers)
    {
    }
    
    public EventContext(TEvent message, IDependencyScope scope, IEnumerable<Type> filters = null, IEnumerable<HandlerWrapper> handlers = null) : base(message, scope, filters, handlers)
    {
    }
}