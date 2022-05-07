using Autofac;
using NMediator.Examples.Base;
using NMediator.Examples.Filters.CommandFilters;
using NMediator.Examples.Filters.EventFilters;
using NMediator.Examples.Filters.ExceptionFilters;
using NMediator.Examples.Filters.MessageFilters;
using NMediator.Examples.Filters.RequestFilters;
using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Services;
using NMediator.Extensions.Autofac;

namespace NMediator.Examples.Autofac;

public static class Program
{
    public static Task Main(string[] args)
    {
        var builder = new ContainerBuilder();

        var logger = new Logger();
        
        builder.RegisterInstance(logger).SingleInstance();
        builder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();
        builder.RegisterType<DoNothingService>().As<IDoNothingService>().InstancePerLifetimeScope();

        builder.RegisterNMediator(config =>
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
        
        var container = builder.Build();

        var mediator = container.Resolve<IMediator>();
        
        return Runner.Run(mediator, logger, "Autofac");
    }
}