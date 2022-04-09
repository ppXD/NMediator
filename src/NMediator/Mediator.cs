using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Ioc;
using NMediator.Middlewares;

namespace NMediator;

public sealed partial class Mediator : IMediator
{
    private readonly IDependencyScope _resolver;
    private readonly MiddlewareProcessor _pipeline;
    private readonly ConcurrentDictionary<Type, List<Type>> _messageHandlerBindings;
    
    public Mediator(IDependencyScope resolver, MiddlewareProcessor pipeline, ConcurrentDictionary<Type, List<Type>> messageHandlerBindings)
    {
        _resolver = resolver;
        _pipeline = pipeline;
        _messageHandlerBindings = messageHandlerBindings;
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

    private async Task SendMessageAsync<TMessage, TContext>(TMessage message, IEnumerable<Type> matchedHandlerTypes, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
        where TContext : IMessageContext<TMessage>
    {
        await ProcessMessage<TMessage, TContext>(message, null, matchedHandlerTypes, cancellationToken).ConfigureAwait(false);
    }

    private async Task<TResponse> SendMessageAsync<TMessage, TResponse, TContext>(TMessage message, IEnumerable<Type> matchedHandlerTypes, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
        where TResponse : class, IResponse
        where TContext : IMessageContext<TMessage>
    {
        return (TResponse) await ProcessMessage<TMessage, TContext>(message, typeof(TResponse), matchedHandlerTypes, cancellationToken).ConfigureAwait(false);
    }

    private async Task<object> ProcessMessage<TMessage, TContext>(TMessage message, Type responseType, IEnumerable<Type> matchedHandlerTypes,
        CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
        where TContext : IMessageContext<TMessage>
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        using var scope = _resolver.BeginScope();

        var baseContext = new MessageContext<TMessage>(message, scope, responseType, FindHandlerTypes<TMessage>(matchedHandlerTypes));

        var context =
            (IMessageContext<TMessage>) Activator.CreateInstance(typeof(TContext), baseContext);
        
        await _pipeline.Process(context, cancellationToken).ConfigureAwait(false);

        return context.Result;
    }
}