using System;
using System.Collections.Generic;
using NMediator.Infrastructure;

namespace NMediator.Context;

public class RequestContext<TRequest> : MessageContext<TRequest>, IRequestContext<TRequest> where TRequest : IRequest
{
    public RequestContext(IMessageContext<TRequest> context) : base(context.Message, context.Scope, context.Filters, context.Handlers)
    {
    }
    
    public RequestContext(TRequest message, IDependencyScope scope, IEnumerable<Type> filters = null, IEnumerable<HandlerWrapper> handlers = null) : base(message, scope, filters, handlers)
    {
    }
}