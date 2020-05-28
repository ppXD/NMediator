using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public class Mediator : IMediator
    {
        private readonly MessageDelegate _messagePipeline;

        private static readonly ConcurrentDictionary<Type, object> CommandHandlers = new ConcurrentDictionary<Type, object>();
        
        public Mediator(MediatorConfiguration mediatorConfiguration)
        {
            Configuration = mediatorConfiguration ?? throw new ArgumentNullException(nameof(mediatorConfiguration));
            
            _messagePipeline = mediatorConfiguration.BuildPipeline();
        }

        public Task SendAsync<TMessage>(TMessage command, CancellationToken cancellationToken = default) 
            where TMessage : ICommand
        {
            if (command == null)
                throw new ArgumentNullException();

            var commandType = command.GetType();

            var handler = (ICommandHandler<TMessage>) CommandHandlers.GetOrAdd(commandType, type =>
            {
                Configuration.MessageBindings.TryGetValue(commandType, out var commandHandlerTypes);

                if (commandHandlerTypes == null || !commandHandlerTypes.Any())
                    throw new NoHandlerFoundException(commandType);
            
                if (commandHandlerTypes.Count > 1)
                {
                    throw new MoreThanOneHandlerException(commandType);
                }

                var handlerType = commandHandlerTypes.Single();

                return Configuration.Resolver.Resolve(handlerType);
            });
            
            _messagePipeline(command);
            
            return handler.Handle(command, cancellationToken);
        }
        
        public MediatorConfiguration Configuration { get; }
    }
}