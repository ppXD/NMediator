using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Requests;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.RequestHandlers;

public class ITestRequestHandler : IRequestHandler<ITestRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(IRequestContext<ITestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestInterfaceRequestHandler : IRequestHandler<TestInterfaceRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(IRequestContext<TestInterfaceRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestInterfaceRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestOneWayRequestHandler : IRequestHandler<TestOneWayRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(IRequestContext<TestOneWayRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestOneWayRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestTwoWayRequestHandler : IRequestHandler<TestTwoWayRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(IRequestContext<TestTwoWayRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestTwoWayRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestAllWayRequestHandler : IRequestHandler<TestAllWayRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(IRequestContext<TestAllWayRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAllWayRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestInheritAllWayRequestHandler : IRequestHandler<TestInheritAllWayRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(IRequestContext<TestInheritAllWayRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestInheritAllWayRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}