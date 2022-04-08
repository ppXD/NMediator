using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.RequestHandlers;

public class TestRequestButOtherResponseHandler : IRequestHandler<TestRequest, TestOtherResponse>
{
    public Task<TestOtherResponse> Handle(IRequestContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.RequestStore.Add(context.Message);
        return Task.FromResult(new TestOtherResponse
        {
            Result = "Test request but other response"
        });
    }
}