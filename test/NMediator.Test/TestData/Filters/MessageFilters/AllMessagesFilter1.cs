using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.MessageFilters;

public class AllMessagesFilter1 : IMessageFilter
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllMessagesFilter1)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(AllMessagesFilter1)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}