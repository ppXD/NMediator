using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.EventFilters;

public class AllEventsFilter1 : IEventFilter
{
    public Task OnExecuting(IEventContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllEventsFilter1)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IEventContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllEventsFilter1)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}