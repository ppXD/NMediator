using NMediator.Examples.Base;
using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Services;
using NMediator.Filters;

namespace NMediator.Examples.Filters.MessageFilters;

public class ExampleCommandMessageFilter : IMessageFilter<ExampleCommand>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public ExampleCommandMessageFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnExecuting(IMessageContext<ExampleCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleCommandMessageFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<ExampleCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleCommandMessageFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}