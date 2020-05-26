using System;
using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public class Mediator : IMediator
    {
        private readonly MessageDelegate _messageDelegate;

        public Mediator(MessageDelegate messageDelegate)
        {
            _messageDelegate = messageDelegate;
        }

        public Task SendAsync<TMessage>(TMessage command, CancellationToken cancellationToken = default) where TMessage : ICommand
        {
            if (command == null)
                throw new ArgumentNullException();

            _messageDelegate(command);
            
            return Task.CompletedTask;
        }
    }

    public class TestMessage : IMessage
    {
        
    }
}