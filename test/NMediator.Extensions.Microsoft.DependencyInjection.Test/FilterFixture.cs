using Microsoft.Extensions.DependencyInjection;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;
using NMediator.Extensions.Microsoft.DependencyInjection.Test.Services;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test;

public class FilterFixture
{
    private readonly Logger _logger;
    private readonly IServiceCollection _services;
    
    public FilterFixture()
    {
        _logger = new Logger();
        _services = new ServiceCollection();
        
        _services.AddSingleton(_logger);
        _services.AddScoped<ILogService, LogService>();
        _services.AddScoped<IDoNothingService, DoNothingService>();
    }
}