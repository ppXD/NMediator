using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.Filters.RequestFilters;

public class ITestRequestFilter : IRequestFilter<ITestRequest>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<ITestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestRequestFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<ITestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestRequestFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}

public class TestAllWayRequestFilter : IRequestFilter<TestAllWayRequest>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<TestAllWayRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAllWayRequestFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestAllWayRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAllWayRequestFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}