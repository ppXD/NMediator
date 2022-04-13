using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.Filters;

public class TestCommandMessageFilter : IMessageFilter<TestCommand>
{
    public Task OnExecuting(IMessageContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandMessageFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandMessageFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}