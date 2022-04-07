using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Examples.AspNetCore.Commands;

namespace NMediator.Examples.AspNetCore.CommandHandlers
{
    public class TestCommandHandlers : ICommandHandler<TestCommand>
    {
        public Task Handle(IMessageContext<TestCommand> context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}