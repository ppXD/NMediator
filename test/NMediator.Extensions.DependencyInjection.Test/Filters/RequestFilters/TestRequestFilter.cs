using System.Threading;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Messages.Requests;
using NMediator.Extensions.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.DependencyInjection.Test.Filters.RequestFilters;

public class TestRequestFilter : IRequestFilter<TestRequest>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public TestRequestFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }

    public Task OnHandlerExecuting(IHandlerExecutingContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestRequestFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestRequestFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}