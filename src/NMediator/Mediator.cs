using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Ioc;
using NMediator.Middleware;

namespace NMediator
{
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
            await SendMessageAsync(command, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> SendAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default) 
            where TCommand : class, ICommand
            where TResponse : class, IResponse
        {
            return await SendMessageAsync<TCommand, TResponse>(command, cancellationToken).ConfigureAwait(false);
        }
        
        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent
        {
            await SendMessageAsync(@event, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
            return await SendMessageAsync<TRequest, TResponse>(request, cancellationToken).ConfigureAwait(false);
        }

        private async Task SendMessageAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : class, IMessage
        {
            await ProcessMessage(message, cancellationToken).ConfigureAwait(false);
        }

        private async Task<TResponse> SendMessageAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : class, IMessage
            where TResponse : class, IResponse
        {
            return (TResponse) await ProcessMessage(message, cancellationToken).ConfigureAwait(false);
        }

        private async Task<object> ProcessMessage<TMessage>(TMessage message,
            CancellationToken cancellationToken = default)
            where TMessage : class, IMessage
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            using var scope = _resolver.BeginScope();

            var context = new MessageContext<TMessage>(message, scope, FindHandlerTypes(typeof(TMessage)));
            
            await _pipeline.Process(context, cancellationToken).ConfigureAwait(false);

            return context.Result;
        }
    }
}