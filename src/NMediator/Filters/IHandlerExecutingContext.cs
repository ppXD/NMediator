namespace NMediator.Filters;

public interface IHandlerExecutingContext<out TMessage> : IFilterContext<TMessage> where TMessage : class, IMessage
{
}