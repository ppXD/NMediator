using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.EventFilters;

public class EventContractFilter : IEventFilter<IEvent>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(EventContractFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(EventContractFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}