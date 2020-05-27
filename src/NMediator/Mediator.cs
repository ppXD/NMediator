using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public class Mediator : IMediator
    {
        private readonly MessageDelegate _messagePipeline;

        public Mediator(MediatorConfiguration mediatorConfiguration)
        {
            Configuration = mediatorConfiguration ?? throw new ArgumentNullException(nameof(mediatorConfiguration));
            
            _messagePipeline = mediatorConfiguration.BuildPipeline();
        }

        public Task SendAsync<TMessage>(TMessage command, CancellationToken cancellationToken = default) where TMessage : ICommand
        {
            if (command == null)
                throw new ArgumentNullException();

            Configuration.MessageBindings.TryGetValue(command.GetType(), out var commandHandlerTypes);

            if (commandHandlerTypes == null || !commandHandlerTypes.Any())
                throw new NoHandlerFoundException(command.GetType());
            
            if (commandHandlerTypes.Count > 1)
            {
                throw new MoreThanOneHandlerException(command.GetType());
            }

            var handlerType = commandHandlerTypes.Single();

            var handler = (ICommandHandler<TMessage>) Configuration.Resolver.Resolve(handlerType) ;

            _messagePipeline(command);
            
            return handler.Handle(command, cancellationToken);
        }
        
        public MediatorConfiguration Configuration { get; }
    }
}