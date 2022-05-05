using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Services;

namespace NMediator.Examples.Handlers.CommandHandlers;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleCommandResponse>
{
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;
    
    public ExampleCommandHandler(ILogService logService, IDoNothingService doNothingService)
    {
        _logService = logService;
        _doNothingService = doNothingService;
    }

    public async Task<ExampleCommandResponse> Handle(ICommandContext<ExampleCommand> context, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(ExampleCommand)}", cancellationToken).ConfigureAwait(false);
        return new ExampleCommandResponse();
    }
}