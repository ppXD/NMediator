namespace NMediator.Filters;

public interface IEventFilter : IHandlerFilter<IEvent>
{
}

public interface IEventFilter<in TEvent> : IHandlerFilter<TEvent>
    where TEvent : class, IEvent
{
}