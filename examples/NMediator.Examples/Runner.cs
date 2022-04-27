using NMediator.Examples.Base;
using NMediator.Examples.Filters.CommandFilters;
using NMediator.Examples.Filters.EventFilters;
using NMediator.Examples.Filters.MessageFilters;
using NMediator.Examples.Filters.RequestFilters;
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

        await mediator.RequestAsync(new ExampleRequest());
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

        await writer.WriteLineAsync();
        
        var publishEquals = publishMessages.SequenceEqual(new[]
        {
            $"{nameof(ExampleMiddleware1)} {nameof(ExampleMiddleware1.OnExecuting)}",
            $"{nameof(ExampleMiddleware2)} {nameof(ExampleMiddleware2.OnExecuting)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuting)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnExecuting)}",
            $"{nameof(ExampleEventFilter)} {nameof(ExampleEventFilter.OnExecuting)}",
            $"{nameof(ExampleEvent)}",
            $"{nameof(ExampleEventFilter)} {nameof(ExampleEventFilter.OnExecuted)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuted)}",
            $"{nameof(ExampleMiddleware2)} {nameof(ExampleMiddleware2.OnExecuted)}",
            $"{nameof(ExampleMiddleware1)} {nameof(ExampleMiddleware1.OnExecuted)}"
        });

        await writer.WriteLineAsync($"Publish ExampleEvent Ordered {publishEquals}");
        
        foreach (var publishMessage in publishMessages)
        {
            await writer.WriteLineAsync(publishMessage);
        }
        
        await writer.WriteLineAsync();
        
        var requestEquals = requestMessages.SequenceEqual(new[]
        {
            $"{nameof(ExampleMiddleware1)} {nameof(ExampleMiddleware1.OnExecuting)}",
            $"{nameof(ExampleMiddleware2)} {nameof(ExampleMiddleware2.OnExecuting)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuting)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnExecuting)}",
            $"{nameof(ExampleRequestFilter)} {nameof(ExampleRequestFilter.OnExecuting)}",
            $"{nameof(ExampleRequest)}",
            $"{nameof(ExampleRequestFilter)} {nameof(ExampleRequestFilter.OnExecuted)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnExecuted)}",
            $"{nameof(ExampleMiddleware2)} {nameof(ExampleMiddleware2.OnExecuted)}",
            $"{nameof(ExampleMiddleware1)} {nameof(ExampleMiddleware1.OnExecuted)}"
        });

        await writer.WriteLineAsync($"Send ExampleRequest Ordered {requestEquals}");
        
        foreach (var requestMessage in requestMessages)
        {
            await writer.WriteLineAsync(requestMessage);
        }
    }

    private static List<string> GetLogMessagesThenClear(Logger logger)
    {
        var messages = new List<string>(logger.Messages);
        
        logger.Messages.Clear();

        return messages;
    }
}