using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters;

public class AllMessageFilter1 : IMessageFilter
{
    public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllMessageFilter1)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllMessageFilter1)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}