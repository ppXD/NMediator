using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestStringCommandHandler : ICommandHandler<TestStringCommand, string>
{
    public Task<string> Handle(ICommandContext<TestStringCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult("Test string response");
    }
}