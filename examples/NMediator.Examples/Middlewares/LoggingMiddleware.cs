using System.Diagnostics;
using NMediator.Context;
using NMediator.Examples.Base;
using NMediator.Middlewares;

namespace NMediator.Examples.Middlewares;

public class LoggingMiddleware : IMiddleware
{
    private readonly Logger _logger;
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    
    public LoggingMiddleware(Logger logger)
    {
        _logger = logger;
    }

    public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _stopwatch.Start();
        
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{_stopwatch.Elapsed.Milliseconds}");
        
        return Task.CompletedTask;
    }
}