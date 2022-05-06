using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.RequestFilters;

public class AllRequestsFilter2 : IRequestFilter
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllRequestsFilter2)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllRequestsFilter2)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}