using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.Filters.RequestFilters;

public class TestAbstractRequestFilter : IRequestFilter<TestAbstractRequest>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<TestAbstractRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractRequestFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestAbstractRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractRequestFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}