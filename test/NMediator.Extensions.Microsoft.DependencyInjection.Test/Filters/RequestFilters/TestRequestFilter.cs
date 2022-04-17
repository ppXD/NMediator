using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Requests;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Filters.RequestFilters;

public class TestRequestFilter : IRequestFilter<TestRequest>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public TestRequestFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnExecuting(IRequestContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestRequestFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IRequestContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestRequestFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}