using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.Requests;

public interface ITestAbstractRequest : IRequest<ITestResponse>, IRequest<TestResponse>, IRequest<TestDerivedResponse>
{
}

public abstract class TestAbstractRequestBase : ITestAbstractRequest
{
}

public class TestAbstractRequest : TestAbstractRequestBase
{
}