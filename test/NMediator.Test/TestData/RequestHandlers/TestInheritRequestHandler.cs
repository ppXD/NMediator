using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Requests;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.RequestHandlers;

public class ITestRequestHandler : IRequestHandler<ITestRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(ITestRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestInterfaceRequestHandler : IRequestHandler<TestInterfaceRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestInterfaceRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestInterfaceRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestOneWayRequestHandler : IRequestHandler<TestOneWayRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestOneWayRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestOneWayRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestTwoWayRequestHandler : IRequestHandler<TestTwoWayRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestTwoWayRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestTwoWayRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestAllWayRequestHandler : IRequestHandler<TestAllWayRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestAllWayRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAllWayRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestInheritAllWayRequestHandler : IRequestHandler<TestInheritAllWayRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestInheritAllWayRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestInheritAllWayRequestHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}