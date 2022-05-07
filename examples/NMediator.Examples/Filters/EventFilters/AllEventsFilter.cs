using NMediator.Examples.Base;
using NMediator.Examples.Services;
using NMediator.Filters;

namespace NMediator.Examples.Filters.EventFilters;

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