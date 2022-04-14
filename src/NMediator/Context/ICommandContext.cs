namespace NMediator.Context;

public interface ICommandContext<out TMessage> : IMessageContext<TMessage> where TMessage : IMessage
{
}