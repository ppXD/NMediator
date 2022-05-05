using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.Filters.RequestFilters;

public class TestRequestFilter : IRequestFilter<TestRequest>
{
    public Task OnHandlerExecuting(HandlerExecutingContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestRequestFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(HandlerExecutedContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestRequestFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}