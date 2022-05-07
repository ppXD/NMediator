using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Events;

namespace NMediator.Test.TestData.EventHandlers;

public class ITestAbstractEventHandler : IEventHandler<ITestAbstractEvent>
{
    public Task Handle(ITestAbstractEvent @event, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestAbstractEventHandler)}");
        return Task.CompletedTask;
    }
}

public class TestAbstractEventBaseHandler : IEventHandler<TestAbstractEventBase>
{
    public Task Handle(TestAbstractEventBase @event, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractEventBaseHandler)}");
        return Task.CompletedTask;
    }
}

public class TestAbstractEventHandler : IEventHandler<TestAbstractEvent>
{
    public Task Handle(TestAbstractEvent @event, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractEventHandler)}");
        return Task.CompletedTask;
    }
}

public class TestAbstractAllInOneEventHandler :
    IEventHandler<ITestAbstractEvent>,
    IEventHandler<TestAbstractEventBase>,
    IEventHandler<TestAbstractEvent>
{
    public Task Handle(ITestAbstractEvent @event, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestAbstractEventHandler)}");
        return Task.CompletedTask;
    }

    public Task Handle(TestAbstractEventBase @event, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractEventBaseHandler)}");
        return Task.CompletedTask;
    }

    public Task Handle(TestAbstractEvent @event, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractEventHandler)}");
        return Task.CompletedTask;
    }
}