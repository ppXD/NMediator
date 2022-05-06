using System.Threading;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Messages.Events;
using NMediator.Extensions.DependencyInjection.Test.Services;

namespace NMediator.Extensions.DependencyInjection.Test.Handlers.EventHandlers;

public class TestEventHandler : IEventHandler<TestEvent>
{
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;

    public TestEventHandler(ILogService logService, IDoNothingService doNothingService)
    {
        _logService = logService;
        _doNothingService = doNothingService;
    }

    public async Task Handle(TestEvent @event, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(TestEvent)}", cancellationToken).ConfigureAwait(false);
    }
}