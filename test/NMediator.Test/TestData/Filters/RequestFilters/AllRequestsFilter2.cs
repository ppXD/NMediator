using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.RequestFilters;

public class AllRequestsFilter2 : IRequestFilter
{
    public Task OnExecuting(IRequestContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllRequestsFilter2)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IRequestContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllRequestsFilter2)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}