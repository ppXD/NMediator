using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.CommandHandlers;

public class ITestAbstractCommandHandler : ICommandHandler<ITestAbstractCommand>
{
    public Task Handle(ICommandContext<ITestAbstractCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestAbstractCommandHandler)}");
        return Task.CompletedTask;
    }
}

public class ITestAbstractHasResponseCommandHandler : ICommandHandler<ITestAbstractCommand, ITestResponse>
{
    public Task<ITestResponse> Handle(ICommandContext<ITestAbstractCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestAbstractHasResponseCommandHandler)}");
        return Task.FromResult((ITestResponse) new TestDerivedResponse());
    }
}

public class TestAbstractCommandBaseHandler : ICommandHandler<TestAbstractCommandBase>
{
    public Task Handle(ICommandContext<TestAbstractCommandBase> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractCommandBaseHandler)}");
        return Task.CompletedTask;
    }
}

public class TestAbstractHasResponseCommandBaseHandler : ICommandHandler<TestAbstractCommandBase, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(ICommandContext<TestAbstractCommandBase> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractHasResponseCommandBaseHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestAbstractCommandHandler : ICommandHandler<TestAbstractCommand>
{
    public Task Handle(ICommandContext<TestAbstractCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractCommandHandler)}");
        return Task.CompletedTask;
    }
}

public class TestAbstractHasResponseCommandHandler : ICommandHandler<TestAbstractCommand, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(ICommandContext<TestAbstractCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractHasResponseCommandHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}