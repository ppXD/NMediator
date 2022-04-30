using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.Commands;

public class TestMultipleImplementationCommand : ICommand, ICommand<TestResponse>, ICommand<TestOtherResponse>
{
    
}