using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestOtherCommandHandler : ICommandHandler<TestOtherCommand>
{
    public Task Handle(ICommandContext<TestOtherCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.CompletedTask;
    }
}