using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.Commands;

public interface ITestAbstractCommand : ICommand, ICommand<TestResponse>, ICommand<TestDerivedResponse>
{
}

public abstract class TestAbstractCommandBase : ITestAbstractCommand
{
}

public class TestAbstractCommand : TestAbstractCommandBase
{
}