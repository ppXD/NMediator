using System;
using System.Collections.Generic;
using NMediator.Infrastructure;

namespace NMediator.Context;

public interface IMessageContext<out TMessage> where TMessage : IMessage
{
    TMessage Message { get; }
    
    IEnumerable<Type> Filters { get; }
    
    IEnumerable<HandlerWrapper> Handlers { get; }
    
    IDependencyScope Scope { get; }
    
    object Result { get; set; }
    
    Exception Exception { get; set; }
    
    bool ExceptionHandled { get; set; }
}