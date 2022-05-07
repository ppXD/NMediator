using System.Threading;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.DependencyInjection.Test.Filters.MessageFilters;

public class TestCommandMessageFilter : IMessageFilter<TestCommand>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public TestCommandMessageFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnHandlerExecuting(IHandlerExecutingContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestCommandMessageFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestCommandMessageFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}