using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class TestOtherCommandFilter : ICommandFilter<TestOtherCommand>
{
    public Task OnExecuting(ICommandContext<TestOtherCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestOtherCommandFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(ICommandContext<TestOtherCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestOtherCommandFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}