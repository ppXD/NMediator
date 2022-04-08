using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.RequestHandlers;

public class TestOtherRequestHandler : IRequestHandler<TestOtherRequest, TestOtherResponse>
{
    public Task<TestOtherResponse> Handle(IRequestContext<TestOtherRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.RequestStore.Add(context.Message);
        return Task.FromResult(new TestOtherResponse());
    }
}