using NMediator.Examples.Base;
using NMediator.Examples.Messages.Requests;
using NMediator.Examples.Services;
using NMediator.Filters;

namespace NMediator.Examples.Filters.RequestFilters;

public class ExampleRequestFilter : IRequestFilter<ExampleRequest>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public ExampleRequestFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }

    public Task OnHandlerExecuting(IHandlerExecutingContext<ExampleRequest> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleRequestFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<ExampleRequest> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleRequestFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}