using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.Commands;

public interface ITestCommand : ICommand, ICommand<TestResponse>, ICommand<TestDerivedResponse>
{
}