namespace NMediator.Context;

public interface IRequestContext<out TRequest> : IMessageContext<TRequest> where TRequest : IRequest
{
}