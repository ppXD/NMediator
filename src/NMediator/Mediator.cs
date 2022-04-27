using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
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
        var commandType = command.GetType();
        
        await ProcessMessage(command, typeof(CommandContext<>).MakeGenericType(commandType), null,
            new[] { typeof(ICommandHandler<>).MakeGenericType(commandType) }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType();

        return (TResponse) await ProcessMessage(command, typeof(CommandContext<>).MakeGenericType(commandType),
            typeof(TResponse), new[] { typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse)) }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> RequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        return (TResponse) await ProcessMessage(request, typeof(RequestContext<>).MakeGenericType(requestType),
            typeof(TResponse), new[] { typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse)) }, cancellationToken).ConfigureAwait(false);
    }
    
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent
    {
        await ProcessMessage(@event, typeof(EventContext<TEvent>), null,
            new[] { typeof(IEventHandler<TEvent>) }, cancellationToken).ConfigureAwait(false);
    }
    
    private async Task<object> ProcessMessage(IMessage message, Type contextType, Type responseType, IEnumerable<Type> handlerTypesToMatch,
        CancellationToken cancellationToken = default)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        using var scope = _configuration.Resolver.BeginScope();

        var context =
            (IMessageContext<IMessage>) Activator.CreateInstance(contextType, message, scope, responseType, 
                _configuration.PipelineConfiguration.FindFilters(message), _configuration.HandlerConfiguration.GetHandlers(message, handlerTypesToMatch));
        
        await _configuration.PipelineConfiguration.PipelineProcessor.Process(context, cancellationToken).ConfigureAwait(false);

        return context?.Result;
    }
}