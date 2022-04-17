using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;
using NMediator.Middlewares;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Middlewares;

public class TestMiddleware1 : IMiddleware
{
    private readonly Logger _logger;
    private readonly IDoNothingService _doNothingService;

    public TestMiddleware1(Logger logger, IDoNothingService doNothingService)
    {
        _logger = logger;
        _doNothingService = doNothingService;
    }
    
    public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestMiddleware1)} {nameof(OnExecuting)}");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.Messages.Add($"{nameof(TestMiddleware1)} {nameof(OnExecuted)}");
        return Task.CompletedTask;
    }
}