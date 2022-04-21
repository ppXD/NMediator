using NMediator.Context;
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
    
    public Task OnExecuting(IEventContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllEventsFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IEventContext<IEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllEventsFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}