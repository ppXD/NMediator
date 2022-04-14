using NMediator.Context;

namespace NMediator.Filters;

public interface IMessageFilter : IExecutionFilter<IMessage, IMessageContext<IMessage>>
{
}

public interface IMessageFilter<TMessage> : IExecutionFilter<TMessage, IMessageContext<TMessage>> 
    where TMessage : class, IMessage
{
}