using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Handlers.CommandHandlers;

public class ThrowExceptionCommandHandler : ICommandHandler<ThrowExceptionCommand>
{
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;

    public ThrowExceptionCommandHandler(ILogService logService, IDoNothingService doNothingService)
    {
        _logService = logService;
        _doNothingService = doNothingService;
    }

    public async Task Handle(ICommandContext<ThrowExceptionCommand> context, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(ThrowExceptionCommand)}", cancellationToken).ConfigureAwait(false);
        throw new Exception();
    }
}