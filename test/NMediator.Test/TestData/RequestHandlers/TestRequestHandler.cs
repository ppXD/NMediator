using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.RequestHandlers
{
    public class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        public Task<TestResponse> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestResponse());
        }
    }
}