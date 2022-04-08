using System;
using System.Collections.Generic;
using NMediator.Ioc;

namespace NMediator.Context;

public interface IMessageContext<out TMessage> where TMessage : IMessage
{
    TMessage Message { get; }
    
    Type ResponseType { get; }
    
    IEnumerable<Type> MessageBindingHandlers { get; }
    
    IDependencyScope Scope { get; }
    
    object Result { get; set; }
}