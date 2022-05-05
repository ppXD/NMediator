using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class AllCommandsFilter2 : ICommandFilter
{
    public Task OnHandlerExecuting(HandlerExecutingContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllCommandsFilter2)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(HandlerExecutedContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllCommandsFilter2)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}