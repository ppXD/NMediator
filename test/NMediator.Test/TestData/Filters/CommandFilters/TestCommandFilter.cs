using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class TestCommandFilter : ICommandFilter<TestCommand>
{
    public Task OnHandlerExecuting(HandlerExecutingContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(HandlerExecutedContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}