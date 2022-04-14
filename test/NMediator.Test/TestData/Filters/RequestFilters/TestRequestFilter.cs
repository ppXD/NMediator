using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.Filters.RequestFilters;

public class TestRequestFilter : IRequestFilter<TestRequest>
{
    public Task OnExecuting(IRequestContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestRequestFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IRequestContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestRequestFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}