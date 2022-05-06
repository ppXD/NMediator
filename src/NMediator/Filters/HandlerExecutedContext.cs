using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace NMediator.Filters;

public class HandlerExecutedContext<TMessage> : FilterContext<TMessage>, IHandlerExecutedContext<TMessage> where TMessage : class, IMessage
{
    private Exception _exception;
    private ExceptionDispatchInfo _exceptionDispatchInfo;
    
    public HandlerExecutedContext(
        TMessage message, 
        IDependencyScope scope, 
        IList<IFilter> filters) 
        : base(message, scope, filters)
    {
    }
    
    public Exception Exception
    {
        get
        {
            if (_exception == null && _exceptionDispatchInfo != null)
                return _exceptionDispatchInfo.SourceException;
            return _exception;
        }
        set
        {
            _exception = value;
            _exceptionDispatchInfo = null;
        }
    }

    public ExceptionDispatchInfo ExceptionDispatchInfo
    {
        get => _exceptionDispatchInfo;
        set
        {
            _exception = null;
            _exceptionDispatchInfo = value;
        }
    }
    
    public bool ExceptionHandled { get; set; }
}