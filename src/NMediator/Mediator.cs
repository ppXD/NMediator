using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using NMediator.Context;

namespace NMediator;

public sealed class Mediator : IMediator
{
    private readonly MediatorConfiguration _configuration;

    public Mediator(MediatorConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand
    {
        await SendMessageAsync<TCommand, CommandContext<TCommand>>(command, 
            new[] { typeof(ICommandHandler<TCommand>), typeof(ICommandHandler<,>) }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> SendAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand
        where TResponse : class, IResponse
    {
        return await SendMessageAsync<TCommand, TResponse, CommandContext<TCommand>>(command,
            new[] { typeof(ICommandHandler<TCommand, TResponse>) }, cancellationToken).ConfigureAwait(false);
    }
    
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        await SendMessageAsync<TEvent, EventContext<TEvent>>(@event, 
                new[] { typeof(IEventHandler<TEvent>) }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : class, IRequest
        where TResponse : class, IResponse
    {
        return await SendMessageAsync<TRequest, TResponse, RequestContext<TRequest>>(request,
            new[] { typeof(IRequestHandler<TRequest, TResponse>) }, cancellationToken).ConfigureAwait(false);
    }

    private async Task SendMessageAsync<TMessage, TContext>(TMessage message, IEnumerable<Type> handlerTypesToMatch, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
        where TContext : IMessageContext<TMessage>
    {
        await ProcessMessage<TMessage, TContext>(message, null, handlerTypesToMatch, cancellationToken).ConfigureAwait(false);
    }

    private async Task<TResponse> SendMessageAsync<TMessage, TResponse, TContext>(TMessage message, IEnumerable<Type> handlerTypesToMatch, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
        where TResponse : class, IResponse
        where TContext : IMessageContext<TMessage>
    {
        return (TResponse) await ProcessMessage<TMessage, TContext>(message, typeof(TResponse), handlerTypesToMatch, cancellationToken).ConfigureAwait(false);
    }

    private async Task<object> ProcessMessage<TMessage, TContext>(TMessage message, Type responseType, IEnumerable<Type> handlerTypesToMatch,
        CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
        where TContext : IMessageContext<TMessage>
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        using var scope = _configuration.Resolver.BeginScope();

        var baseContext = new MessageContext<TMessage>(message, scope, responseType, 
            _configuration.PipelineConfiguration.FindFilters<TMessage>(), _configuration.HandlerConfiguration.GetHandlers<TMessage>(handlerTypesToMatch));

        var context =
            (IMessageContext<TMessage>) Activator.CreateInstance(typeof(TContext), baseContext);
        
        await _configuration.PipelineConfiguration.PipelineProcessor.Process(context, cancellationToken).ConfigureAwait(false);

        return context?.Result;
    }
}