using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestCommandHandler : ICommandHandler<TestCommand>
{
    public Task Handle(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.CommandStore.Add(context.Message);
        return Task.CompletedTask;
    }
}