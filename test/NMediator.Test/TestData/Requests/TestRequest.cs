using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.Requests;

public class TestRequest : IRequest<TestResponse>, IRequest<TestOtherResponse>
{
    public string Message { get; set; }
}