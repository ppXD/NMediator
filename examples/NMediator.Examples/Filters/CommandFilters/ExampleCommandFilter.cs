using NMediator.Context;
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
    
    public Task OnExecuting(ICommandContext<ExampleCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleCommandFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(ICommandContext<ExampleCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleCommandFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}