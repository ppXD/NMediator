using System;
using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public class Mediator : IMediator
    {
        private readonly MessageDelegate _messagePipeline;

        public Mediator(MediatorConfiguration configuration)
        {
            Configuration = configuration;
            
            _messagePipeline = configuration.BuildPipeline();
        }

        public Task SendAsync<TMessage>(TMessage command, CancellationToken cancellationToken = default) where TMessage : ICommand
        {
            if (command == null)
                throw new ArgumentNullException();

            _messagePipeline(command);
            
            return Task.CompletedTask;
        }
        
        public MediatorConfiguration Configuration { get; }
    }
}