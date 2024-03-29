using System.Threading;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Messages.Events;
using NMediator.Extensions.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.DependencyInjection.Test.Filters.EventFilters;

public class TestEventFilter : IEventFilter<TestEvent>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public TestEventFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }

    public Task OnHandlerExecuting(IHandlerExecutingContext<TestEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestEventFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestEventFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}