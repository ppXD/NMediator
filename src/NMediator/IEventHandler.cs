namespace NMediator
{
    public interface IEventHandler<in TEvent> : IHandler<TEvent>
        where TEvent : class, IEvent
    {
    }
}