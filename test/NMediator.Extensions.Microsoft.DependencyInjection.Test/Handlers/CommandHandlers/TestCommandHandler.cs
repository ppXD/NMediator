using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Handlers.CommandHandlers;

public class TestCommandHandler : ICommandHandler<TestCommand, TestCommandResponse>
{
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;
    
    public TestCommandHandler(ILogService logService, IDoNothingService doNothingService)
    {
        _logService = logService;
        _doNothingService = doNothingService;
    }

    public async Task<TestCommandResponse> Handle(ICommandContext<TestCommand> context, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(TestCommand)}", cancellationToken).ConfigureAwait(false);
        return new TestCommandResponse();
    }
}