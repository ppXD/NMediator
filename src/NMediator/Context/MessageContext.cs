using System;
using System.Collections.Generic;
using NMediator.Ioc;

namespace NMediator.Context;

public class MessageContext<TMessage> : IMessageContext<TMessage>
    where TMessage : IMessage
{
    protected MessageContext(TMessage message, IDependencyScope scope, Type responseType = null, IEnumerable<Type> filters = null, IEnumerable<Type> messageBindingHandlers = null)
    {
        Scope = scope;
        Message = message;
        ResponseType = responseType;
        Filters = filters;
        MessageBindingHandlers = messageBindingHandlers;
    }
    
    public TMessage Message { get; }
    
    public Type ResponseType { get; }

    public IEnumerable<Type> Filters { get; }
    
    public IEnumerable<Type> MessageBindingHandlers { get; }

    public IDependencyScope Scope { get; }
    
    public object Result { get; set; }
    
    public Exception Exception { get; set; }
    
    public bool ExceptionHandled { get; set; }
}