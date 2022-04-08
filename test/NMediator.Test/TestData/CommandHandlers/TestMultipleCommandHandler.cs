using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestMultipleCommandHandler : 
    ICommandHandler<TestCommand>,
    ICommandHandler<TestOtherCommand, TestResponse>
{
    public Task Handle(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.CommandStore.Add(context.Message);
        return Task.CompletedTask;
    }

    public Task<TestResponse> Handle(ICommandContext<TestOtherCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.CommandStore.Add(context.Message);
        return Task.FromResult(new TestResponse());
    }
}