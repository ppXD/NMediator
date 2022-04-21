using NMediator.Context;
using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Messages.Requests;
using NMediator.Examples.Services;

namespace NMediator.Examples.Handlers.CommandHandlers;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleResponse>
{
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;
    
    public ExampleCommandHandler(ILogService logService, IDoNothingService doNothingService)
    {
        _logService = logService;
        _doNothingService = doNothingService;
    }

    public async Task<ExampleResponse> Handle(ICommandContext<ExampleCommand> context, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(ExampleCommand)}", cancellationToken).ConfigureAwait(false);
        return new ExampleResponse();
    }
}