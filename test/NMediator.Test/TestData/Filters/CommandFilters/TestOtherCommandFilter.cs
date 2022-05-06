using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class TestOtherCommandFilter : ICommandFilter<TestOtherCommand>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<TestOtherCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestOtherCommandFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestOtherCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestOtherCommandFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}