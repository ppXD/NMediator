using NMediator.Examples.Messages.Events;
using NMediator.Examples.Services;

namespace NMediator.Examples.Handlers.EventHandlers;

public class ExampleEventHandler : IEventHandler<ExampleEvent>
{
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;

    public ExampleEventHandler(ILogService logService, IDoNothingService doNothingService)
    {
        _logService = logService;
        _doNothingService = doNothingService;
    }

    public async Task Handle(ExampleEvent @event, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(ExampleEvent)}", cancellationToken).ConfigureAwait(false);
    }
}