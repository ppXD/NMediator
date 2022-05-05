using NMediator.Examples.Base;
using NMediator.Examples.Services;
using NMediator.Filters;

namespace NMediator.Examples.Filters.ExceptionFilters;

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
        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}