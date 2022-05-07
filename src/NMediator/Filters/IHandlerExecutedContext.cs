using System;
using System.Runtime.ExceptionServices;

namespace NMediator.Filters;

public interface IHandlerExecutedContext<out TMessage> : IFilterContext<TMessage> where TMessage : class, IMessage
{
    public Exception Exception { get; set; }
    
    public ExceptionDispatchInfo ExceptionDispatchInfo { get; set; }
    
    public bool ExceptionHandled { get; set; }
}