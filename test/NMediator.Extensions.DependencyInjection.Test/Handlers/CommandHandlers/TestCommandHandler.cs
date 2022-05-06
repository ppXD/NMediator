using System.Threading;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Messages.Commands;
using NMediator.Extensions.DependencyInjection.Test.Services;

namespace NMediator.Extensions.DependencyInjection.Test.Handlers.CommandHandlers;

public class TestCommandHandler : ICommandHandler<TestCommand, TestCommandResponse>
{
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;
    
    public TestCommandHandler(ILogService logService, IDoNothingService doNothingService)
    {
        _logService = logService;
        _doNothingService = doNothingService;
    }

    public async Task<TestCommandResponse> Handle(TestCommand command, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(TestCommand)}", cancellationToken).ConfigureAwait(false);
        return new TestCommandResponse();
    }
}