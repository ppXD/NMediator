using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class ITestCommandFilter : ICommandFilter<ITestCommand>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<ITestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestCommandFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<ITestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestCommandFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}

public class TestCommandAllWayCommandFilter : ICommandFilter<TestCommandAllWayCommand>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<TestCommandAllWayCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandAllWayCommandFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestCommandAllWayCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandAllWayCommandFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}