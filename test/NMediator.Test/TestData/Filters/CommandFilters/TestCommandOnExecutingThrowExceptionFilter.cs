using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class TestCommandOnExecutingThrowExceptionFilter : ICommandFilter<TestCommand>
{
    public Task OnHandlerExecuting(HandlerExecutingContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandOnExecutingThrowExceptionFilter)} {nameof(OnHandlerExecuting)}");
        throw new Exception();
    }

    public Task OnHandlerExecuted(HandlerExecutedContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandOnExecutingThrowExceptionFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}