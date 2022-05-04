using System;
using System.Collections.Generic;
using NMediator.Infrastructure;

namespace NMediator.Context;

public class MessageContext<TMessage> : IMessageContext<TMessage>
    where TMessage : IMessage
{
    protected MessageContext(TMessage message, IDependencyScope scope, IEnumerable<Type> filters = null, IEnumerable<HandlerWrapper> handlers = null)
    {
        Scope = scope;
        Message = message;
        Filters = filters;
        Handlers = handlers;
    }
    
    public TMessage Message { get; }
    
    public IEnumerable<Type> Filters { get; }
    
    public IEnumerable<HandlerWrapper> Handlers { get; }

    public IDependencyScope Scope { get; }
    
    public object Result { get; set; }
    
    public Exception Exception { get; set; }
    
    public bool ExceptionHandled { get; set; }
}