using System;
using System.Collections.Generic;
using NMediator.Infrastructure;
using NMediator.Ioc;

namespace NMediator.Context;

public interface IMessageContext<out TMessage> where TMessage : IMessage
{
    TMessage Message { get; }
    
    Type ResponseType { get; }
    
    IEnumerable<Type> Filters { get; }
    
    IEnumerable<HandlerWrapper> Handlers { get; }
    
    IDependencyScope Scope { get; }
    
    object Result { get; set; }
    
    Exception Exception { get; set; }
    
    bool ExceptionHandled { get; set; }
}