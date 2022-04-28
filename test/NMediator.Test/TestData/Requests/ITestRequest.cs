namespace NMediator.Test.TestData.Requests;

public interface ITestRequest : IRequest<TestResponse>, IRequest<TestOtherResponse>
{
}