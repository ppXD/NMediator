namespace NMediator;

public interface IRequestHandler<in TRequest, TResponse> : IHandler<TRequest, TResponse>
    where TRequest : class, IRequest
    where TResponse : class, IResponse
{
}