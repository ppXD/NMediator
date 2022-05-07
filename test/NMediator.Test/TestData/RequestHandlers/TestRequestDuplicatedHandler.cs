using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Requests;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.RequestHandlers;

public class TestRequestDuplicatedHandler : IRequestHandler<TestRequest, TestResponse>
{
    public Task<TestResponse> Handle(TestRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(request);
        return Task.FromResult(new TestResponse
        {
            Result = "Test duplicated handler"
        });
    }
}