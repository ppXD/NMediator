using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestMultipleCommandHandler : 
    ICommandHandler<TestCommand>,
    ICommandHandler<TestHasResponseCommand, TestResponse>
{
    public Task Handle(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.CompletedTask;
    }

    public Task<TestResponse> Handle(ICommandContext<TestHasResponseCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestResponse());
    }
}