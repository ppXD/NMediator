using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Requests;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.RequestHandlers;

public class TestRequestButOtherResponseHandler : IRequestHandler<TestRequest, TestOtherResponse>
{
    public Task<TestOtherResponse> Handle(TestRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(request);
        return Task.FromResult(new TestOtherResponse
        {
            Result = "Test request but other response"
        });
    }
}