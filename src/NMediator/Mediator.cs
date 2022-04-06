using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        public Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
            where TCommand : class, ICommand
        {
            return SendMessageAsync(command, cancellationToken);
        }

        public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent
        {
            return SendMessageAsync(@event, cancellationToken);
        }

        public Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
            return SendMessageAsync<TRequest, TResponse>(request, cancellationToken);
        }

        private Task SendMessageAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : class, IMessage
        {
            return ProcessMessage(message, cancellationToken);
        }

        private Task<TResponse> SendMessageAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : class, IMessage
            where TResponse : class, IResponse
        {
            return Task.FromResult((TResponse) ProcessMessage(message, cancellationToken).Result);
        }

        private async Task<object> ProcessMessage<TMessage>(TMessage message,
            CancellationToken cancellationToken = default)
            where TMessage : class, IMessage
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var handlers = FindHandlerTypes(typeof(TMessage));

            using var scope = _resolver.BeginScope();
            
            await _pipeline.Process(scope, cancellationToken).ConfigureAwait(false);
            
            foreach (var handler in handlers)
            {
                await ((IHandler<TMessage>)scope.Resolve(handler)).Handle(message, cancellationToken);
            }

            return null;
        }
    }
}