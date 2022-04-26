using System;
using System.Collections.Generic;
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
        var commandType = command.GetType();
        
        await SendMessageAsync(command, typeof(CommandContext<>).MakeGenericType(commandType),
            new[] { typeof(ICommandHandler<>).MakeGenericType(commandType) }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType();

        return await SendMessageAsync(command, typeof(CommandContext<>).MakeGenericType(commandType),
            new[] { typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse)) }, cancellationToken).ConfigureAwait(false);
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent
    {
        await SendMessageAsync(@event, typeof(EventContext<TEvent>),
            new[] { typeof(IEventHandler<TEvent>) }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> RequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        return await SendMessageAsync(request, typeof(RequestContext<>).MakeGenericType(requestType),
            new[] { typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse)) }, cancellationToken).ConfigureAwait(false);
    }
    
    private async Task SendMessageAsync(IMessage message, Type contextType, IEnumerable<Type> handlerTypesToMatch, CancellationToken cancellationToken = default)
    {
        await ProcessMessage(message, contextType, null, handlerTypesToMatch, cancellationToken).ConfigureAwait(false);
    }

    private async Task<TResponse> SendMessageAsync<TResponse>(IMessage<TResponse> message, Type contextType, IEnumerable<Type> handlerTypesToMatch, CancellationToken cancellationToken = default)
    {
        return (TResponse) await ProcessMessage(message, contextType, typeof(TResponse), handlerTypesToMatch, cancellationToken).ConfigureAwait(false);
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