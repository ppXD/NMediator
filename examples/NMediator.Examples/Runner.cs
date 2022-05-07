using NMediator.Examples.Base;
using NMediator.Examples.Filters.CommandFilters;
using NMediator.Examples.Filters.EventFilters;
using NMediator.Examples.Filters.MessageFilters;
using NMediator.Examples.Filters.RequestFilters;
using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Messages.Events;
using NMediator.Examples.Messages.Requests;

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
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleCommandMessageFilter)} {nameof(ExampleCommandMessageFilter.OnHandlerExecuting)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleCommandFilter)} {nameof(ExampleCommandFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleCommand)}",
            $"{nameof(ExampleCommandFilter)} {nameof(ExampleCommandFilter.OnHandlerExecuted)}",
            $"{nameof(AllCommandsFilter)} {nameof(AllCommandsFilter.OnHandlerExecuted)}",
            $"{nameof(ExampleCommandMessageFilter)} {nameof(ExampleCommandMessageFilter.OnHandlerExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuted)}"
        });

        await writer.WriteLineAsync($"Send ExampleCommand Ordered {sendEquals}");
        
        foreach (var sendMessage in sendMessages)
        {
            await writer.WriteLineAsync(sendMessage);
        }

        await writer.WriteLineAsync();
        
        var publishEquals = publishMessages.SequenceEqual(new[]
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuting)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleEventFilter)} {nameof(ExampleEventFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleEvent)}",
            $"{nameof(ExampleEventFilter)} {nameof(ExampleEventFilter.OnHandlerExecuted)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnHandlerExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuted)}"
        });

        await writer.WriteLineAsync($"Publish ExampleEvent Ordered {publishEquals}");
        
        foreach (var publishMessage in publishMessages)
        {
            await writer.WriteLineAsync(publishMessage);
        }
        
        await writer.WriteLineAsync();
        
        var requestEquals = requestMessages.SequenceEqual(new[]
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuting)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleRequestFilter)} {nameof(ExampleRequestFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleRequest)}",
            $"{nameof(ExampleRequestFilter)} {nameof(ExampleRequestFilter.OnHandlerExecuted)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnHandlerExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuted)}"
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