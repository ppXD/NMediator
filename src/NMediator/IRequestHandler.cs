using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public interface IRequestHandler<in TRequest, TResponse>
        where TRequest : class, IRequest
        where TResponse : class, IResponse
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}