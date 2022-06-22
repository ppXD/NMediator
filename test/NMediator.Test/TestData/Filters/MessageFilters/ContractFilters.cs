using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.MessageFilters;

public class CommandContractMessageFilter : IMessageFilter<ICommand>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(CommandContractMessageFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(CommandContractMessageFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}

public class RequestContractMessageFilter : IMessageFilter<IRequest>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(RequestContractMessageFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(RequestContractMessageFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}

public class EventContractMessageFilter : IMessageFilter<IEvent>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(EventContractMessageFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(EventContractMessageFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}