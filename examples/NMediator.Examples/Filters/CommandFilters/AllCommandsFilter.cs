using NMediator.Examples.Base;
using NMediator.Examples.Services;
using NMediator.Filters;

namespace NMediator.Examples.Filters.CommandFilters;

public class AllCommandsFilter : ICommandFilter
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public AllCommandsFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }

    public Task OnHandlerExecuting(IHandlerExecutingContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllCommandsFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllCommandsFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}