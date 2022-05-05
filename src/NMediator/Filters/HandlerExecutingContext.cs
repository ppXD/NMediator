using System.Collections.Generic;

namespace NMediator.Filters;

public class HandlerExecutingContext<TMessage> : FilterContext<TMessage> where TMessage : class, IMessage
{
    public HandlerExecutingContext(
        TMessage message, 
        IDependencyScope scope, 
        IList<IFilter> filters) 
        : base(message, scope, filters)
    {
    }
    
    public object Result { get; set; }
}