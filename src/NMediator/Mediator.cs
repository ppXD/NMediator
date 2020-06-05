using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NMediator
{
    public class Mediator : IMediator
    {
        private readonly MessageDelegate _messagePipeline;

        private static readonly ConcurrentDictionary<Type, List<object>> Handlers = new ConcurrentDictionary<Type, List<object>>();
        
        public Mediator(MediatorConfiguration mediatorConfiguration)
        {
            Configuration = mediatorConfiguration ?? throw new ArgumentNullException(nameof(mediatorConfiguration));
            
            _messagePipeline = mediatorConfiguration.BuildPipeline();
        }

        public Task SendAsync<TMessage>(TMessage command, CancellationToken cancellationToken = default) 
            where TMessage : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandType = command.GetType();

            var handlers = FindHandlers(commandType);
            
            if (handlers.Count > 1)
                throw new MoreThanOneHandlerException(commandType);

            var handler = (ICommandHandler<TMessage>) handlers.Single();
            
            _messagePipeline(command);
            
            return handler.Handle(command, cancellationToken);
        }

        public Task PublishAsync<TMessage>(TMessage @event, CancellationToken cancellationToken = default)
            where TMessage : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));
            
            var eventType = @event.GetType();

            var handlers = FindHandlers(eventType);
            
            _messagePipeline(@event);

            var tasks = handlers.Select(h => ((IEventHandler<TMessage>) h).Handle(@event, cancellationToken)).ToList();

            return Task.WhenAll(tasks);
        }

        public Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            
            var requestType = request.GetType();
            
            var handlers = FindHandlers(requestType);
            
            if (handlers.Count > 1)
                throw new MoreThanOneHandlerException(requestType);

            var handler = (IRequestHandler<TRequest, TResponse>) handlers.Single();
            
            _messagePipeline(requestType);
            
            return handler.Handle(request, cancellationToken);
        }

        private List<object> FindHandlers(Type messageType)
        {
            return Handlers.GetOrAdd(messageType, type =>
            {
                Configuration.MessageBindings.TryGetValue(messageType, out var handlerTypes);

                if (handlerTypes == null || !handlerTypes.Any())
                    throw new NoHandlerFoundException(messageType);

                return handlerTypes.Select(t => Configuration.Resolver.Resolve(t)).ToList();
            });
        }
        
        public MediatorConfiguration Configuration { get; }
    }
}