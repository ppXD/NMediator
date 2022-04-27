namespace NMediator.Context;

public interface IEventContext<out TMessage> : IMessageContext<TMessage> where TMessage : IEvent
{
}