namespace NMediator.Filters;

public interface IEventFilter : IHandlerFilter<IEvent>
{
}

public interface IEventFilter<TEvent> : IHandlerFilter<TEvent>
    where TEvent : class, IEvent
{
}