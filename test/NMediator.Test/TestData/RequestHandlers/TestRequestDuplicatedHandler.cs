using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.RequestHandlers;

public class TestRequestDuplicatedHandler : IRequestHandler<TestRequest, TestResponse>
{
    public Task<TestResponse> Handle(IRequestContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestResponse
        {
            Result = "Test response"
        });
    }
}