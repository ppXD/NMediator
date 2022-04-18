using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
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
    
    public Task OnExecuting(IEventContext<TestEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestEventFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IEventContext<TestEvent> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestEventFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}