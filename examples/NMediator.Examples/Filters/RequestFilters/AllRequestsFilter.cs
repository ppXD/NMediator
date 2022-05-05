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
    
    public Task OnExecuting(IRequestContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllRequestsFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IRequestContext<IRequest> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllRequestsFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}