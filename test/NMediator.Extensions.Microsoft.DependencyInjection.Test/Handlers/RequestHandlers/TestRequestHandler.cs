using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Messages.Requests;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Handlers.RequestHandlers;

public class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
{
    private readonly ILogService _logService;
    private readonly IDoNothingService _doNothingService;
    
    public TestRequestHandler(ILogService logService, IDoNothingService doNothingService)
    {
        _logService = logService;
        _doNothingService = doNothingService;
    }
    
    public async Task<TestResponse> Handle(IRequestContext<TestRequest> context, CancellationToken cancellationToken = default)
    {
        await _logService.LogMessage($"{nameof(TestRequest)}", cancellationToken).ConfigureAwait(false);
        return new TestResponse();
    }
}