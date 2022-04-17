using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Filters.MessageFilters;

public class AllMessagesFilter : IMessageFilter
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public AllMessagesFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllMessagesFilter)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(AllMessagesFilter)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}