using System.Threading;
using System.Threading.Tasks;
using NMediator.Examples.AspNetCore.Commands;

namespace NMediator.Examples.AspNetCore.CommandHandlers
{
    public class TestCommandHandlers : ICommandHandler<TestCommand>
    {
        public Task Handle(TestCommand command, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}