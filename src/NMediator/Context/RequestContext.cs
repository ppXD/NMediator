using System;
using System.Collections.Generic;
using NMediator.Ioc;

namespace NMediator.Context;

public class RequestContext<TRequest> : MessageContext<TRequest>, IRequestContext<TRequest> where TRequest : IMessage
{
    public RequestContext(IMessageContext<TRequest> context) : base(context.Message, context.Scope, context.ResponseType, context.MessageBindingHandlers)
    {
    }
    
    public RequestContext(TRequest message, IDependencyScope scope, Type responseType = null, IEnumerable<Type> messageBindingHandlers = null) : base(message, scope, responseType, messageBindingHandlers)
    {
    }
}