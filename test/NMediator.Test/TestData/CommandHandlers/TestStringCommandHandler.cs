using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestStringCommandHandler : ICommandHandler<TestStringCommand, string>
{
    public Task<string> Handle(TestStringCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(command);
        return Task.FromResult("Test string response");
    }
}