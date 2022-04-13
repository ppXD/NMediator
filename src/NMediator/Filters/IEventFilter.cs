using NMediator.Context;

namespace NMediator.Filters;

public interface IEventFilter : IExecutionFilter<IEvent, IEventContext<IEvent>>
{
}

public interface IEventFilter<TEvent> : IExecutionFilter<TEvent, IEventContext<TEvent>> 
    where TEvent : class, IEvent
{
}