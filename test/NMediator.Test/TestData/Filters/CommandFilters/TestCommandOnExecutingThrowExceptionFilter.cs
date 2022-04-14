using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.Filters.CommandFilters;

public class TestCommandOnExecutingThrowExceptionFilter : ICommandFilter<TestCommand>
{
    public Task OnExecuting(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandOnExecutingThrowExceptionFilter)} {nameof(OnExecuting)}");
        throw new Exception();
    }

    public Task OnExecuted(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandOnExecutingThrowExceptionFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}