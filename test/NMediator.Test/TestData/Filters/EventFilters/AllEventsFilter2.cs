using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.EventFilters;

public class AllEventsFilter2 : IEventFilter
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllEventsFilter2)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllEventsFilter2)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}