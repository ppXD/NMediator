using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.Commands;

public interface ITestAbstractCommand : ICommand
{
}

public abstract class TestAbstractCommandBase : ITestAbstractCommand, ICommand<TestResponse>, ICommand<TestDerivedResponse>
{
}

public class TestAbstractCommand : TestAbstractCommandBase
{
}