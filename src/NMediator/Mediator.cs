using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

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
        
        var commandType = command.GetType();
        
        await ProcessMessage(command, typeof(CommandContext<>).MakeGenericType(commandType), null, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        
        var commandType = command.GetType();

        return (TResponse)await ProcessMessage(command, typeof(CommandContext<>).MakeGenericType(commandType),
            typeof(TResponse), cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> RequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        
        var requestType = request.GetType();

        return (TResponse)await ProcessMessage(request, typeof(RequestContext<>).MakeGenericType(requestType),
            typeof(TResponse), cancellationToken).ConfigureAwait(false);
    }
    
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent, new()
    {
        await ProcessMessage(@event, typeof(EventContext<TEvent>), null, cancellationToken).ConfigureAwait(false);
    }
    
    private async Task<object> ProcessMessage(IMessage message, Type contextType, Type responseType, 
        CancellationToken cancellationToken = default)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        using var scope = _configuration.Resolver.BeginScope();

        var context =
            (IMessageContext<IMessage>) Activator.CreateInstance(contextType, message, scope, 
                _configuration.PipelineConfiguration.FindFilters(message), _configuration.HandlerConfiguration.GetHandlers(message, responseType));
        
        await _configuration.PipelineConfiguration.PipelineProcessor.Process(context, cancellationToken).ConfigureAwait(false);

        return context?.Result;
    }
}