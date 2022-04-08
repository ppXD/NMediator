namespace NMediator.Context;

public interface IRequestContext<out TMessage> : IMessageContext<TMessage> where TMessage : IMessage
{
}