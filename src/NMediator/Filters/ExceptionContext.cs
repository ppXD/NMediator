using System.Collections.Generic;

namespace NMediator.Filters;

public class ExceptionContext<TMessage> : FilterContext<TMessage> where TMessage : class, IMessage
{
    public ExceptionContext(TMessage message, IDependencyScope scope, IList<IFilter> filters) : base(message, scope, filters)
    {
    }
    
    public bool ExceptionHandled { get; set; }
}