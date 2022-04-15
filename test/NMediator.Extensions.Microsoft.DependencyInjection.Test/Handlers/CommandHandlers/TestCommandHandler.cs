using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Handlers.CommandHandlers;

public class TestCommandHandler : ICommandHandler<TestCommand>
{
    private readonly ILogService _logService;

    public TestCommandHandler(ILogService logService)
    {
        _logService = logService;
    }

    public async Task Handle(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(TestCommand)}", cancellationToken).ConfigureAwait(false);
    }
}