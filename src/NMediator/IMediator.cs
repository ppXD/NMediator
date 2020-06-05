using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public interface IMediator
    {
        Task SendAsync<TMessage>(TMessage command, CancellationToken cancellationToken = default)
            where TMessage : ICommand;

        Task PublishAsync<TMessage>(TMessage @event, CancellationToken cancellationToken = default)
            where TMessage : IEvent;
        
        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request,
            CancellationToken cancellationToken = default)
            where TRequest : class, IRequest
            where TResponse : class, IResponse;
    }
}