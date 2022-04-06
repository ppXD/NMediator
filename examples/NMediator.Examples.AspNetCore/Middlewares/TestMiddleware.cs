using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using NMediator.Examples.AspNetCore.Commands;
using IMiddleware = NMediator.Middleware.IMiddleware;

namespace NMediator.Examples.AspNetCore.Middlewares
{
    public class TestMiddleware : IMiddleware
    {
        public async Task OnExecuting(object message, CancellationToken cancellationToken)
        {
            if (message is TestCommand command)
            {
                command.Message = "Invoked middleware";
            }
        }

        public Task OnExecuted(object message, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    public class TestMiddleware1
    {
        private readonly RequestDelegate _next;
        
        public TestMiddleware1(RequestDelegate requestDelegate)
        {
            this._next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await Console.Out.WriteAsync("begin");
            await _next.Invoke(context);
            await Console.Out.WriteAsync("end");
        }
    }
    
    public class TestFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.Out.Write("end");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.Out.Write("start");
        }
    }
}