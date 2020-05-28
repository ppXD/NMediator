using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NMediator.Examples.AspNetCore.Commands;

namespace NMediator.Examples.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("send")]
        public async Task<IActionResult> Send()
        {
            await _mediator.SendAsync(new TestCommand
            {
                Message = "Hello world"
            });
            
            return Ok();
        }
    }
}