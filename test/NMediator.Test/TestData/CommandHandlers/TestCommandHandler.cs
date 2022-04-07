using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.CommandHandlers
{
    public class TestCommandHandler : ICommandHandler<TestCommand, TestResponse>
    {
        public Task<TestResponse> Handle(IMessageContext<TestCommand> context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new TestResponse { Result = "" });
        }
    }
}