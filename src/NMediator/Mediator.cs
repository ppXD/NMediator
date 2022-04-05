using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NMediator
{
    public sealed partial class Mediator : IMediator
    {
        private static readonly ConcurrentDictionary<Type, List<object>> Handlers = new ConcurrentDictionary<Type, List<object>>();
        
        public Mediator(MediatorConfiguration mediatorConfiguration)
        {
            Configuration = mediatorConfiguration ?? throw new ArgumentNullException(nameof(mediatorConfiguration));
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
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var handlers = FindHandlerTypes(typeof(TMessage));

            using (var scope = Configuration.Resolver.BeginScope())
            {
                foreach (var handler in handlers)
                {
                    ((IHandler<TMessage>)scope.Resolve(handler)).Handle(message, cancellationToken);
                }
            }
            
            return Task.CompletedTask;
        }

        private Task<TResponse> SendMessageAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : class, IMessage
            where TResponse : class, IResponse
        {
            return Task.FromResult((TResponse)default);
        }
        
        public MediatorConfiguration Configuration { get; }
    }
}