using NMediator.Examples.Base;
using NMediator.Examples.Services;
using NMediator.Filters;

namespace NMediator.Examples.Filters.RequestFilters;

public class AllRequestsFilter : IRequestFilter
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public AllRequestsFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnHandlerExecuting(IHandlerExecutingContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllRequestsFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllRequestsFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}