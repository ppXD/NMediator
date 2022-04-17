using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Filters.MessageFilters;

public class TestCommandMessageFilter : IMessageFilter<TestCommand>
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public TestCommandMessageFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnExecuting(IMessageContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestCommandMessageFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestCommandMessageFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}