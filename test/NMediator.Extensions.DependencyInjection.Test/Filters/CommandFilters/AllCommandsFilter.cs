using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.DependencyInjection.Test.Filters.CommandFilters;

public class AllCommandsFilter : ICommandFilter
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public AllCommandsFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }

    public Task OnExecuting(ICommandContext<IBasicCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllCommandsFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(ICommandContext<IBasicCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllCommandsFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}