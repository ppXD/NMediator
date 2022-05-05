using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.RequestFilters;

public class AllRequestsFilter1 : IRequestFilter
{
    public Task OnHandlerExecuting(HandlerExecutingContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllRequestsFilter1)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(HandlerExecutedContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllRequestsFilter1)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}