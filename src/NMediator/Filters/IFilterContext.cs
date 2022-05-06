using System;
using System.Collections.Generic;

namespace NMediator.Filters;

public interface IFilterContext<out TMessage> where TMessage : IMessage
{
    public TMessage Message { get; }
    
    public IDependencyScope Scope { get; }
    
    public IList<IFilter> Filters { get; }
    
    public object Result { get; set; }
}