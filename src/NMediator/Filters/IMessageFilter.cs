namespace NMediator.Filters;

public interface IMessageFilter : IHandlerFilter
{
}

public interface IMessageFilter<in TMessage> : IHandlerFilter<TMessage>
    where TMessage : class, IMessage
{
}