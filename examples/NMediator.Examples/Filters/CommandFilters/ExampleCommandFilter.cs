using NMediator.Examples.Base;
using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Services;
using NMediator.Filters;

namespace NMediator.Examples.Filters.CommandFilters;

public class ExampleCommandFilter : ICommandFilter<ExampleCommand>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public ExampleCommandFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }

    public Task OnHandlerExecuting(IHandlerExecutingContext<ExampleCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleCommandFilter)} {nameof(OnHandlerExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnHandlerExecuted(IHandlerExecutedContext<ExampleCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleCommandFilter)} {nameof(OnHandlerExecuted)}");
        return Task.CompletedTask;
    }
}