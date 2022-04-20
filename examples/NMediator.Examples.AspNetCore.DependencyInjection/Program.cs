using NMediator;
using NMediator.Examples.Base;
using NMediator.Examples.Services;
using NMediator.Examples.Middlewares;
using NMediator.Examples.Filters.CommandFilters;
using NMediator.Examples.Filters.EventFilters;
using NMediator.Examples.Filters.ExceptionFilters;
using NMediator.Examples.Filters.MessageFilters;
using NMediator.Examples.Filters.RequestFilters;
using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Messages.Events;
using NMediator.Examples.Messages.Requests;
using NMediator.Extensions.Microsoft.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("command/send", async (IMediator mediator) =>
{
    await mediator.SendAsync(new ExampleCommand());
});

app.MapGet("event/publish", async (IMediator mediator) =>
{
    await mediator.PublishAsync(new ExampleEvent());
});

app.MapGet("request/send", async (IMediator mediator) => await mediator.RequestAsync<ExampleRequest, ExampleResponse>(new ExampleRequest()));

app.Run();