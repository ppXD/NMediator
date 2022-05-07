using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.CommandHandlers;

public class ITestCommandHandler : ICommandHandler<ITestCommand>
{
    public Task Handle(ITestCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestCommandHandler)}");
        return Task.CompletedTask;
    }
}

public class ITestCommandHasResponseHandler : ICommandHandler<ITestCommand, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(ITestCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestCommandHasResponseHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestInterfaceCommandHandler : ICommandHandler<TestInterfaceCommand>
{
    public Task Handle(TestInterfaceCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestInterfaceCommandHandler)}");
        return Task.CompletedTask;
    }
}

public class TestInterfaceHasResponseCommandHandler : ICommandHandler<TestInterfaceCommand, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestInterfaceCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestInterfaceHasResponseCommandHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestCommandOneWayCommandHandler : ICommandHandler<TestCommandOneWayCommand>
{
    public Task Handle(TestCommandOneWayCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandOneWayCommandHandler)}");
        return Task.CompletedTask;
    }
}

public class TestCommandOneWayHasResponseCommandHandler : ICommandHandler<TestCommandOneWayCommand, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestCommandOneWayCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandOneWayHasResponseCommandHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestCommandTwoWayCommandHandler : ICommandHandler<TestCommandTwoWayCommand>
{
    public Task Handle(TestCommandTwoWayCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandTwoWayCommandHandler)}");
        return Task.CompletedTask;
    }
}

public class TestCommandTwoWayHasResponseCommandHandler : ICommandHandler<TestCommandTwoWayCommand, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestCommandTwoWayCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandTwoWayHasResponseCommandHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestCommandAllWayCommandHandler : ICommandHandler<TestCommandAllWayCommand>
{
    public Task Handle(TestCommandAllWayCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandAllWayCommandHandler)}");
        return Task.CompletedTask;
    }
}

public class TestCommandAllWayHasResponseCommandHandler : ICommandHandler<TestCommandAllWayCommand, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestCommandAllWayCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestCommandAllWayHasResponseCommandHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestInheritAllWayCommandHandler : ICommandHandler<TestInheritAllWayCommand>
{
    public Task Handle(TestInheritAllWayCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestInheritAllWayCommandHandler)}");
        return Task.CompletedTask;
    }
}

public class TestInheritAllWayHasResponseCommandHandler : ICommandHandler<TestInheritAllWayCommand, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestInheritAllWayCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestInheritAllWayHasResponseCommandHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}