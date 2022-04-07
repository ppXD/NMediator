using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.RequestHandlers
{
    public class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        public Task<TestResponse> Handle(IMessageContext<TestRequest> context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new TestResponse());
        }
    }
}