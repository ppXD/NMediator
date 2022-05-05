using System.Collections.Generic;

namespace NMediator.Filters;

public abstract class FilterContext<TMessage> where TMessage : class, IMessage
{
    protected FilterContext(
        TMessage message,
        IDependencyScope scope,
        IList<IFilter> filters)
    {
        Message = message;
        Scope = scope;
        Filters = filters;
    }
    
    public TMessage Message { get; }
    
    public IDependencyScope Scope { get; }
    
    public IList<IFilter> Filters { get; }
}