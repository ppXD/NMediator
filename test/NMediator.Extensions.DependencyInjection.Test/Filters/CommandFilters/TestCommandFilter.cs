using System.Threading;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.DependencyInjection.Test.Filters.CommandFilters;

public class TestCommandFilter : ICommandFilter<TestCommand>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public TestCommandFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }

    public Task OnHandlerExecuting(IHandlerExecutingContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestCommandFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestCommandFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}