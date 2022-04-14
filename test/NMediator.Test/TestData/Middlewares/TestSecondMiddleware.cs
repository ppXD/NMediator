using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Middlewares;

namespace NMediator.Test.TestData.Middlewares;

public class TestSecondMiddleware : IMiddleware
{
    public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestSecondMiddleware)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestSecondMiddleware)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}