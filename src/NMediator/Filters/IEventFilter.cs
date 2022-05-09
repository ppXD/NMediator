namespace NMediator.Filters;

public interface IEventFilter : IMessageFilter<IEvent>
{
}

public interface IEventFilter<in TEvent> : IMessageFilter<TEvent>
    where TEvent : class, IEvent
{
}