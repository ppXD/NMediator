using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.CommandHandlers
{
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task Handle(TestCommand command, CancellationToken cancellationToken)
        {
            TestStore.CommandStore.Add(command);
            
            return Task.CompletedTask;
        }
    }
}