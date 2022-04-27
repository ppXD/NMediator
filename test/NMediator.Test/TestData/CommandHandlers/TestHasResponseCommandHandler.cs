using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestHasResponseCommandHandler : ICommandHandler<TestHasResponseCommand, TestResponse>
{
    public Task<TestResponse> Handle(ICommandContext<TestHasResponseCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestResponse
        {
            Result = "Test command response"
        });
    }
}