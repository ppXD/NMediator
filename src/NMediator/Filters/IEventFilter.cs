using NMediator.Context;

namespace NMediator.Filters;

public interface IEventFilter : IExecutionFilter<IEvent, IMessageContext<IEvent>>
{
}

public interface IEventFilter<TEvent> : IExecutionFilter<TEvent, IMessageContext<TEvent>> 
    where TEvent : class, IEvent
{
}