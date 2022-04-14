using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class TestCommandOnExecutedThrowExceptionFilter : ICommandFilter<TestCommand>
{
    public Task OnExecuting(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandOnExecutedThrowExceptionFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandOnExecutedThrowExceptionFilter)} {nameof(OnExecuted)}");
        throw new Exception();
    }
}