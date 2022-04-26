using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator;

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    Task<TResponse> Handle(IRequestContext<TRequest> context, CancellationToken cancellationToken = default);
}