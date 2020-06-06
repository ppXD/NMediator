using System.Threading.Tasks;
using NMediator.Test.TestData.RequestHandlers;
using NMediator.Test.TestData.Requests;
using Shouldly;
using Xunit;

namespace NMediator.Test.TestRequestHandlers
{
    public class BasicRequestHandlerShouldBeWork : TestBase
    {
        [Fact]
        public async Task Test()
        {
            var mediator = new MediatorConfiguration()
                .RegisterHandler<TestRequestHandler>()
                .CreateMediator();
            
            var request = new TestRequest
            {
                Message = "test request"
            };

            var response = await mediator.RequestAsync<TestRequest, TestResponse>(request);
            
            response.ShouldNotBeNull();
        }
    }
}