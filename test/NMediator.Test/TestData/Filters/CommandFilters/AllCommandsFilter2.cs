using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class AllCommandsFilter2 : ICommandFilter
{
    public Task OnExecuting(ICommandContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllCommandsFilter2)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(ICommandContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllCommandsFilter2)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}