using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public interface IMediator
    {
        Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : class, ICommand;

        Task<TResponse> SendAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : class, ICommand
            where TResponse : class, IResponse;
        
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent;
        
        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request,
            CancellationToken cancellationToken = default)
            where TRequest : class, IRequest
            where TResponse : class, IResponse;
    }
}