using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.Filters.MessageFilters;

public class TestCommandMessageFilter : IMessageFilter<TestCommand>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandMessageFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandMessageFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}