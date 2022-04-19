using NMediator.Context;
using NMediator.Examples.Base;
using NMediator.Examples.Services;
using NMediator.Middlewares;

namespace NMediator.Examples.Middlewares;

public class ExampleMiddleware2 : IMiddleware
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public ExampleMiddleware2(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }

    public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleMiddleware2)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(ExampleMiddleware2)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}