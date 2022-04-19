using NMediator.Context;
using NMediator.Examples.Base;
using NMediator.Examples.Messages.Events;
using NMediator.Examples.Services;
using NMediator.Filters;

namespace NMediator.Examples.Filters.EventFilters;

public class ExampleEventFilter : IEventFilter<ExampleEvent>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public ExampleEventFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnExecuting(IEventContext<ExampleEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleEventFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IEventContext<ExampleEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleEventFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}