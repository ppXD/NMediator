using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestCommandHasResponseHandler : ICommandHandler<TestCommand, TestResponse>
{
    public Task<TestResponse> Handle(IMessageContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.CommandStore.Add(context.Message);
        return Task.FromResult(new TestResponse());
    }
}