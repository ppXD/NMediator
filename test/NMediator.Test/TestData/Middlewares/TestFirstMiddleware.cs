using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Middlewares;

namespace NMediator.Test.TestData.Middlewares;

public class TestFirstMiddleware : IMiddleware
{
    public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestFirstMiddleware)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestFirstMiddleware)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}