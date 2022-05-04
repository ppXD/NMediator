namespace NMediator.Filters;

public interface IEventFilter
{
}

public interface IEventFilter<TEvent>
    where TEvent : class, IEvent
{
}