using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class AllCommandsFilter1 : ICommandFilter
{
    public Task OnExecuting(ICommandContext<IBasicCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllCommandsFilter1)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(ICommandContext<IBasicCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllCommandsFilter1)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}