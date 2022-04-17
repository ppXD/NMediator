using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;
using NMediator.Filters;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Filters.ExceptionFilters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public ExceptionFilter(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnException(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExceptionFilter)} {nameof(OnException)}");
        return Task.CompletedTask;
    }
}