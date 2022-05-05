using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Requests;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.RequestHandlers;

public class TestDerivedRequestHandler1 : IRequestHandler<TestDerivedRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestDerivedRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(request);
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestDerivedRequestHandler2 : IRequestHandler<TestDerivedRequest, TestResponse>
{
    public Task<TestResponse> Handle(TestDerivedRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(request);
        return Task.FromResult(new TestResponse());
    }
}

public class TestDerivedRequestHandler3 : 
    IRequestHandler<TestDerivedRequest, TestResponse>,
    IRequestHandler<TestDerivedRequest, TestDerivedResponse>
{
    public Task<TestResponse> Handle(TestDerivedRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(request);
        return Task.FromResult(new TestResponse());
    }

    Task<TestDerivedResponse> IRequestHandler<TestDerivedRequest, TestDerivedResponse>.Handle(TestDerivedRequest request, CancellationToken cancellationToken)
    {
        TestStore.Stores.Add(request);
        return Task.FromResult(new TestDerivedResponse());
    }
}