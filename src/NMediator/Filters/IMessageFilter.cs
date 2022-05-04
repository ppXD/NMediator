namespace NMediator.Filters;

public interface IMessageFilter
{
}

public interface IMessageFilter<TMessage>
    where TMessage : class, IMessage
{
}