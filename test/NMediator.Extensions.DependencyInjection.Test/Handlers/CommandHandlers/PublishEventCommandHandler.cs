using System.Threading;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.DependencyInjection.Test.Messages.Events;
using NMediator.Extensions.DependencyInjection.Test.Services;

namespace NMediator.Extensions.DependencyInjection.Test.Handlers.CommandHandlers;

public class PublishEventCommandHandler : ICommandHandler<PublishEventCommand>
{
    private readonly IMediator _mediator;
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;

    public PublishEventCommandHandler(IMediator mediator, ILogService logService, IDoNothingService doNothingService)
    {
        _mediator = mediator;
        _logService = logService;
        _doNothingService = doNothingService;
    }

    public async Task Handle(PublishEventCommand command, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(PublishEventCommand)}", cancellationToken).ConfigureAwait(false);
        await _mediator.PublishAsync(new TestEvent(), cancellationToken).ConfigureAwait(false);
    }
}