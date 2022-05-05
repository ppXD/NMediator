using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Internal;

namespace NMediator;

public class Mediator : IMediator
{
    private readonly MediatorConfiguration _configuration;

    public Mediator(MediatorConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        
        await ProcessMessage(command, null, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return (TResponse)await ProcessMessage(command, typeof(TResponse), cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> RequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        
        return (TResponse)await ProcessMessage(request, typeof(TResponse), cancellationToken).ConfigureAwait(false);
    }
    
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent, new()
    {
        await ProcessMessage(@event, null, cancellationToken).ConfigureAwait(false);
    }

    private async Task<object> ProcessMessage(IMessage message, Type responseType, CancellationToken cancellationToken = default)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        using var scope = _configuration.Resolver.BeginScope();

        var messageProcessor = Activator.CreateInstance(typeof(MessageProcessor<>).MakeGenericType(message.GetType()),
            message, scope, _configuration.FilterConfiguration.FindFilters(message), _configuration.HandlerConfiguration.GetHandlers(message, responseType)) as dynamic;

        await messageProcessor.Process(cancellationToken);
        
        return null;
    }
}