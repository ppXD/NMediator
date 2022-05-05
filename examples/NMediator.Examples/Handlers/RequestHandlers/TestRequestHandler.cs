using NMediator.Examples.Messages.Requests;
using NMediator.Examples.Services;

namespace NMediator.Examples.Handlers.RequestHandlers;

public class ExampleRequestHandler : IRequestHandler<ExampleRequest, ExampleResponse>
{
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;
    
    public ExampleRequestHandler(ILogService logService, IDoNothingService doNothingService)
    {
        _logService = logService;
        _doNothingService = doNothingService;
    }
    
    public async Task<ExampleResponse> Handle(IRequestContext<ExampleRequest> context, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(ExampleRequest)}", cancellationToken).ConfigureAwait(false);
        return new ExampleResponse();
    }
}