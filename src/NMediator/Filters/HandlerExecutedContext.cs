using System.Collections.Generic;

namespace NMediator.Filters;

public class HandlerExecutedContext<TMessage> : FilterContext<TMessage> where TMessage : class, IMessage
{
    public HandlerExecutedContext(
        TMessage message, 
        IDependencyScope scope, 
        IList<IFilter> filters) 
        : base(message, scope, filters)
    {
    }
    
    public object Result { get; set; }
    
    public bool ExceptionHandled { get; set; }
}