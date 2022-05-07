using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestHasResponseCommandHandler : ICommandHandler<TestHasResponseCommand, TestResponse>
{
    public Task<TestResponse> Handle(TestHasResponseCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(command);
        return Task.FromResult(new TestResponse
        {
            Result = "Test command response"
        });
    }
}