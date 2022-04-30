using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Requests;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.RequestHandlers;

public class TestRequestAllInOneHandler :
    IRequestHandler<TestRequest, TestResponse>,
    IRequestHandler<TestOtherRequest, TestResponse>
{
    public Task<TestResponse> Handle(IRequestContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestResponse
        {
            Result = "Test response for test request"
        });
    }
    
    public Task<TestResponse> Handle(IRequestContext<TestOtherRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestResponse
        {
            Result = "Test response for test other request"
        });
    }
}

public class TestOtherRequestAllInOneHandler :
    IRequestHandler<TestRequest, TestOtherResponse>,
    IRequestHandler<TestOtherRequest, TestOtherResponse>
{
    public Task<TestOtherResponse> Handle(IRequestContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestOtherResponse
        {
            Result = "Test other response for test request"
        });
    }

    public Task<TestOtherResponse> Handle(IRequestContext<TestOtherRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestOtherResponse
        {
            Result = "Test other response for test other request"
        });
    }
}