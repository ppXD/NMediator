using NMediator.Examples.Base;
using NMediator.Examples.Services;

namespace NMediator.Examples.Middlewares;

public class ExampleMiddleware1 : IMiddleware
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public ExampleMiddleware1(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleMiddleware1)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleMiddleware1)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}