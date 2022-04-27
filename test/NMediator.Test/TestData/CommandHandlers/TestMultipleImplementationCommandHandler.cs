using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestMultipleImplementationNonResponseCommandHandler : ICommandHandler<TestMultipleImplementationCommand>
{
    public Task Handle(ICommandContext<TestMultipleImplementationCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.CompletedTask;
    }
}

public class TestMultipleImplementationHasResponseCommandHandler : ICommandHandler<TestMultipleImplementationCommand, TestResponse>
{
    public Task<TestResponse> Handle(ICommandContext<TestMultipleImplementationCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestResponse());
    }
}