namespace NMediator.Context;

public interface IContext<out TMessage> where TMessage : IMessage
{
    TMessage Message { get; }
}