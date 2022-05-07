using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.DependencyInjection.Test.Services;

namespace NMediator.Extensions.DependencyInjection.Test.Handlers.CommandHandlers;

public class ThrowExceptionCommandHandler : ICommandHandler<ThrowExceptionCommand>
{
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;

    public ThrowExceptionCommandHandler(ILogService logService, IDoNothingService doNothingService)
    {
        _logService = logService;
        _doNothingService = doNothingService;
    }

    public async Task Handle(ThrowExceptionCommand command, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(ThrowExceptionCommand)}", cancellationToken).ConfigureAwait(false);
        throw new Exception();
    }
}