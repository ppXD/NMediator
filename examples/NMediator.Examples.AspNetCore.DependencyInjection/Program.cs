using NMediator.Examples.Base;
using NMediator.Examples.Services;
using NMediator.Examples.Middlewares;
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
        .UseMiddleware<ExampleMiddleware1>()
        .UseMiddleware<ExampleMiddleware2>()
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