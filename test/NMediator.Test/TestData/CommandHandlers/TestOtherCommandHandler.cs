using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestOtherCommandHandler : ICommandHandler<TestOtherCommand>
{
    public Task Handle(TestOtherCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(command);
        return Task.CompletedTask;
    }
}