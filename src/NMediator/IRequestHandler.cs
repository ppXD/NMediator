using NMediator.Context;

namespace NMediator;

public interface IRequestHandler<in TRequest, TResponse> : IHandler<TRequest, TResponse, IRequestContext<TRequest>>
    where TRequest : class, IRequest<TResponse>
{
}