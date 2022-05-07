using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Requests;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.RequestHandlers;

public class ITestAbstractRequestAsITestResponseHandler : IRequestHandler<ITestAbstractRequest, ITestResponse>
{
    public Task<ITestResponse> Handle(ITestAbstractRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestAbstractRequestAsITestResponseHandler)}");
        return Task.FromResult((ITestResponse) new TestDerivedResponse());
    }
}

public class ITestAbstractRequestAsTestResponseHandler : IRequestHandler<ITestAbstractRequest, TestResponse>
{
    public Task<TestResponse> Handle(ITestAbstractRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestAbstractRequestAsTestResponseHandler)}");
        return Task.FromResult(new TestResponse());
    }
}

public class ITestAbstractRequestAsDerivedResponseHandler : IRequestHandler<ITestAbstractRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(ITestAbstractRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ITestAbstractRequestAsDerivedResponseHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestAbstractRequestBaseAsTestResponseHandler : IRequestHandler<TestAbstractRequestBase, TestResponse>
{
    public Task<TestResponse> Handle(TestAbstractRequestBase request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractRequestBaseAsTestResponseHandler)}");
        return Task.FromResult(new TestResponse());
    }
}

public class TestAbstractRequestBaseAsDerivedResponseHandler : IRequestHandler<TestAbstractRequestBase, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestAbstractRequestBase request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractRequestBaseAsDerivedResponseHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestAbstractRequestAsTestResponseHandler : IRequestHandler<TestAbstractRequest, TestResponse>
{
    public Task<TestResponse> Handle(TestAbstractRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractRequestAsTestResponseHandler)}");
        return Task.FromResult(new TestResponse());
    }
}

public class TestAbstractRequestAsDerivedResponseHandler : IRequestHandler<TestAbstractRequest, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(TestAbstractRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestAbstractRequestAsDerivedResponseHandler)}");
        return Task.FromResult(new TestDerivedResponse());
    }
}