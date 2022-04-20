using NMediator.Examples.Base;
using NMediator.Examples.Filters.CommandFilters;
using NMediator.Examples.Filters.MessageFilters;
using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Messages.Events;
using NMediator.Examples.Messages.Requests;
using NMediator.Examples.Middlewares;

namespace NMediator.Examples;

public static class Runner
{
    public static async Task Run(IMediator mediator, Logger logger, string exampleProject)
    {
        var writer = Console.Out;
        
        await writer.WriteLineAsync("===============");
        await writer.WriteLineAsync(exampleProject);
        await writer.WriteLineAsync("===============");
        await writer.WriteLineAsync();
        
        await mediator.SendAsync(new ExampleCommand());
        var sendMessages = GetLogMessagesThenClear(logger);
        
        await mediator.PublishAsync(new ExampleEvent());
        var publishMessages = GetLogMessagesThenClear(logger);

        await mediator.RequestAsync<ExampleRequest, ExampleResponse>(new ExampleRequest());
        var requestMessages = GetLogMessagesThenClear(logger);
        
        var sendEquals = sendMessages.SequenceEqual(new[]
        {
            $"{nameof(ExampleMiddleware1)} {nameof(ExampleMiddleware1.OnExecuting)}",
            $"{nameof(ExampleMiddleware2)} {nameof(ExampleMiddleware2.OnExecuting)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuting)}",
            $"{nameof(ExampleCommandMessageFilter)} {nameof(ExampleCommandMessageFilter.OnExecuting)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnExecuting)}",
            $"{nameof(ExampleCommandFilter)} {nameof(ExampleCommandFilter.OnExecuting)}",
            $"{nameof(ExampleCommand)}",
            $"{nameof(ExampleCommandFilter)} {nameof(ExampleCommandFilter.OnExecuted)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnExecuted)}",
            $"{nameof(ExampleCommandMessageFilter)} {nameof(ExampleCommandMessageFilter.OnExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuted)}",
            $"{nameof(ExampleMiddleware2)} {nameof(ExampleMiddleware2.OnExecuted)}",
            $"{nameof(ExampleMiddleware1)} {nameof(ExampleMiddleware1.OnExecuted)}"
        });

        await writer.WriteLineAsync($"Send ExampleCommand Ordered {sendEquals}");
        
        foreach (var sendMessage in sendMessages)
        {
            await writer.WriteLineAsync(sendMessage);
        }
    }

    private static List<string> GetLogMessagesThenClear(Logger logger)
    {
        var messages = new List<string>(logger.Messages);
        
        logger.Messages.Clear();

        return messages;
    }
}