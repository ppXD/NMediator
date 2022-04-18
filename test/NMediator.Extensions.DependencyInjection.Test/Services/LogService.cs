using System.Threading;
using System.Threading.Tasks;
using NMediator.Extensions.DependencyInjection.Test.Base;

namespace NMediator.Extensions.DependencyInjection.Test.Services;

public interface ILogService
{
    Task LogMessage(string msg, CancellationToken cancellationToken);
}

public class LogService : ILogService
{
    private readonly Logger _logger;

    public LogService(Logger logger)
    {
        _logger = logger;
    }

    public Task LogMessage(string msg, CancellationToken cancellationToken)
    {
        _logger.Messages.Add(msg);
        return Task.CompletedTask;
    }
}