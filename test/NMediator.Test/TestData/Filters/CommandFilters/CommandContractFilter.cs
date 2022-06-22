using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class CommandContractFilter : ICommandFilter<ICommand>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(CommandContractFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(CommandContractFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}