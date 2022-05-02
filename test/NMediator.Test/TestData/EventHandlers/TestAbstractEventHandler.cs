using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Events;

namespace NMediator.Test.TestData.EventHandlers;

public class ITestAbstractEventHandler : IEventHandler<ITestAbstractEvent>
{
    public Task Handle(IEventContext<ITestAbstractEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestAbstractEventHandler)}");
        return Task.CompletedTask;
    }
}

public class TestAbstractEventBaseHandler : IEventHandler<TestAbstractEventBase>
{
    public Task Handle(IEventContext<TestAbstractEventBase> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractEventBaseHandler)}");
        return Task.CompletedTask;
    }
}

public class TestAbstractEventHandler : IEventHandler<TestAbstractEvent>
{
    public Task Handle(IEventContext<TestAbstractEvent> context, CancellationToken cancellationToken = default)
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
    public Task Handle(IEventContext<ITestAbstractEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestAbstractEventHandler)}");
        return Task.CompletedTask;
    }

    public Task Handle(IEventContext<TestAbstractEventBase> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractEventBaseHandler)}");
        return Task.CompletedTask;
    }

    public Task Handle(IEventContext<TestAbstractEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractEventHandler)}");
        return Task.CompletedTask;
    }
}