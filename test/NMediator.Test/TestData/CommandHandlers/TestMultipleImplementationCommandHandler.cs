using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestMultipleImplementationNonResponseCommandHandler1 : ICommandHandler<TestMultipleImplementationCommand>
{
    public Task Handle(TestMultipleImplementationCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestMultipleImplementationNonResponseCommandHandler1)}");
        return Task.CompletedTask;
    }
}

public class TestMultipleImplementationNonResponseCommandHandler2 : ICommandHandler<TestMultipleImplementationCommand>
{
    public Task Handle(TestMultipleImplementationCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestMultipleImplementationNonResponseCommandHandler2)}");
        return Task.CompletedTask;
    }
}

public class TestMultipleImplementationHasResponseCommandHandler : ICommandHandler<TestMultipleImplementationCommand, TestResponse>
{
    public Task<TestResponse> Handle(TestMultipleImplementationCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestMultipleImplementationHasResponseCommandHandler)}");
        return Task.FromResult(new TestResponse());
    }
}