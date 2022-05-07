using Microsoft.AspNetCore.Mvc;
using NMediator.Examples.Base;
using NMediator.Examples.Filters.CommandFilters;
using NMediator.Examples.Filters.EventFilters;
using NMediator.Examples.Filters.MessageFilters;
using NMediator.Examples.Filters.RequestFilters;
using NMediator.Examples.Messages.Commands;
using NMediator.Examples.Messages.Events;
using NMediator.Examples.Messages.Requests;

namespace NMediator.Examples.AspNetCore.Api;

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
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuting)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleEventFilter)} {nameof(ExampleEventFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleEvent)}",
            $"{nameof(ExampleEventFilter)} {nameof(ExampleEventFilter.OnHandlerExecuted)}",
            $"{nameof(AllEventsFilter)} {nameof(AllEventsFilter.OnHandlerExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuted)}"
        });

        _logger.Messages.Clear();
        
        return Ok(new {equals, messages});
    }
    
    [Route("request/send"), HttpGet]
    public async Task<IActionResult> SendRequest()
    {
        var response = await _mediator.RequestAsync(new ExampleRequest());
        
        var messages = new List<string>(_logger.Messages);

        var equals = _logger.Messages.SequenceEqual(new[]
        {
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuting)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleRequestFilter)} {nameof(ExampleRequestFilter.OnHandlerExecuting)}",
            $"{nameof(ExampleRequest)}",
            $"{nameof(ExampleRequestFilter)} {nameof(ExampleRequestFilter.OnHandlerExecuted)}",
            $"{nameof(AllRequestsFilter)} {nameof(AllRequestsFilter.OnHandlerExecuted)}",
            $"{nameof(AllMessagesFilter)} {nameof(AllMessagesFilter.OnHandlerExecuted)}"
        });

        _logger.Messages.Clear();
        
        return Ok(new {equals, messages});
    }
}