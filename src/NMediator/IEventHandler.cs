using NMediator.Context;

namespace NMediator;

public interface IEventHandler<in TEvent> : IHandler<TEvent, IEventContext<TEvent>>
    where TEvent : class, IEvent, new()
{
}