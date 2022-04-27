using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.Commands;

public class TestMultipleImplementationCommand : ICommand, ICommand<TestResponse>, ICommand<TestOtherResponse>
{
    
}