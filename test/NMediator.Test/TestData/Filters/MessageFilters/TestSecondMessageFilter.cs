using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters;

public class TestSecondMessageFilter : IMessageFilter
{
    public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestSecondMessageFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(TestSecondMessageFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}