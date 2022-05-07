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

    public Task OnHandlerExecuting(IHandlerExecutingContext<ExampleEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleEventFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<ExampleEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleEventFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}