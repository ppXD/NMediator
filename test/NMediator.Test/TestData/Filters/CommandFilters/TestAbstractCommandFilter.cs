using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class TestAbstractCommandFilter : ICommandFilter<TestAbstractCommandBase>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<TestAbstractCommandBase> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractCommandFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestAbstractCommandBase> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractCommandFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}