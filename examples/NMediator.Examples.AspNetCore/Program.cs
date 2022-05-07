using Microsoft.Extensions.DependencyInjection;
using NMediator.Examples.Base;
using NMediator.Examples.Filters.CommandFilters;
using NMediator.Examples.Filters.EventFilters;
using NMediator.Examples.Filters.ExceptionFilters;
using NMediator.Examples.Filters.MessageFilters;
using NMediator.Examples.Filters.RequestFilters;
using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Services;
using NMediator.Extensions.Microsoft.DependencyInjection;

namespace NMediator.Examples.AspNetCore;

public static class Program
{
    public static Task Main(string[] args)
    {
        var services = new ServiceCollection();

        var logger = new Logger();
        
        services.AddSingleton(logger);
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<IDoNothingService, DoNothingService>();

        services.AddNMediator(config =>
        {
            config
                .UseFilter<AllMessagesFilter>()
                .UseFilter<ExampleCommandMessageFilter>()
                .UseFilter<AllCommandsFilter>()
                .UseFilter<ExampleCommandFilter>()
                .UseFilter<AllEventsFilter>()
                .UseFilter<ExampleEventFilter>()
                .UseFilter<AllRequestsFilter>()
                .UseFilter<ExampleRequestFilter>()
                .UseFilter<ExceptionFilter>();
        }, typeof(ExampleCommand).Assembly);
        
        var provider = services.BuildServiceProvider();

        var mediator = provider.GetRequiredService<IMediator>();
        
        return Runner.Run(mediator, logger, "AspNetCore");
    }
}