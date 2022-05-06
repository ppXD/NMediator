using System.Threading;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.DependencyInjection.Test.Filters.EventFilters;

public class AllEventsFilter : IEventFilter
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public AllEventsFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }

    public Task OnHandlerExecuting(IHandlerExecutingContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllEventsFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllEventsFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}