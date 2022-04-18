using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.DependencyInjection.Test.Filters.RequestFilters;

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