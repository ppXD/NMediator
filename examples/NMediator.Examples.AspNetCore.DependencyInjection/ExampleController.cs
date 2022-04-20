using Microsoft.AspNetCore.Mvc;
using NMediator.Examples.Base;
using NMediator.Examples.Filters.CommandFilters;
using NMediator.Examples.Filters.EventFilters;
using NMediator.Examples.Filters.MessageFilters;
using NMediator.Examples.Filters.RequestFilters;
using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Messages.Events;
using NMediator.Examples.Messages.Requests;
using NMediator.Examples.Middlewares;

namespace NMediator.Examples.AspNetCore.DependencyInjection;

[Route("[controller]")]
public class ExampleController : ControllerBase
{
    private readonly Logger _logger;
    private readonly IMediator _mediator;

    public ExampleController(Logger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [Route("command/send"), HttpGet]
    public async Task<IActionResult> SendCommand()
    {
        await _mediator.SendAsync(new ExampleCommand());
        
        var messages = new List<string>(_logger.Messages);

        var equals = _logger.Messages.SequenceEqual(new[]
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

        _logger.Messages.Clear();
        
        return Ok(new {equals, messages});
    }
    
    [Route("event/publish"), HttpGet]
    public async Task<IActionResult> PublishEvent()
    {
        await _mediator.PublishAsync(new ExampleEvent());
        
        var messages = new List<string>(_logger.Messages);

        var equals = _logger.Messages.SequenceEqual(new[]
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

        _logger.Messages.Clear();
        
        return Ok(new {equals, messages});
    }
    
    [Route("request/send"), HttpGet]
    public async Task<IActionResult> SendRequest()
    {
        var response = await _mediator.RequestAsync<ExampleRequest, ExampleResponse>(new ExampleRequest());
        
        var messages = new List<string>(_logger.Messages);

        var equals = _logger.Messages.SequenceEqual(new[]
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

        _logger.Messages.Clear();
        
        return Ok(new {equals, messages});
    }
}