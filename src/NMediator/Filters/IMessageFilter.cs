namespace NMediator.Filters;

public interface IMessageFilter : IHandlerFilter<IMessage>
{
}

public interface IMessageFilter<TMessage> : IHandlerFilter<TMessage>
    where TMessage : class, IMessage
{
}