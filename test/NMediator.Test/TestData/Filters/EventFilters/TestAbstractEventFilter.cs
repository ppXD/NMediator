using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;
using NMediator.Test.TestData.Events;

namespace NMediator.Test.TestData.Filters.EventFilters;

public class TestAbstractEventFilter : IEventFilter<TestAbstractEvent>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<TestAbstractEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractEventFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestAbstractEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractEventFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}