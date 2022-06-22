using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.RequestFilters;

public class RequestContractFilter : IRequestFilter<IRequest>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(RequestContractFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(RequestContractFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}