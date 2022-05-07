using NMediator.Examples.Base;
using NMediator.Examples.Services;
using NMediator.Examples.Filters.CommandFilters;
using NMediator.Examples.Filters.EventFilters;
using NMediator.Examples.Filters.ExceptionFilters;
using NMediator.Examples.Filters.MessageFilters;
using NMediator.Examples.Filters.RequestFilters;
using NMediator.Examples.Messages.Commands;
using NMediator.Extensions.Microsoft.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton(new Logger());
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IDoNothingService, DoNothingService>();

builder.Services.AddNMediator(config =>
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

var app = builder.Build();

app.MapControllers();

app.Run();