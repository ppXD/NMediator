using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Filters.CommandFilters;

public class TestCommandFilter : ICommandFilter<TestCommand>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public TestCommandFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnExecuting(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestCommandFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestCommandFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}