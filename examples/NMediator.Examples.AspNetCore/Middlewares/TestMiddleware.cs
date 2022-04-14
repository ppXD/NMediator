using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NMediator.Context;
using NMediator.Examples.AspNetCore.Commands;
using IMiddleware = NMediator.Middlewares.IMiddleware;

namespace NMediator.Examples.AspNetCore.Middlewares
{
    public class TestMiddleware : IMiddleware
    {
        public async Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken)
        {
            if (context.Message is TestCommand command)
            {
                command.Message = "Invoked middleware";
            }
        }

        public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
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
    
    public class TestMiddleware2
    {
        private readonly RequestDelegate _next;
        
        public TestMiddleware2(RequestDelegate requestDelegate)
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
    
    public class TestFilter1 : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.Out.Write("start");
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.Out.Write("end");
        }
    }
    
    public class TestFilter2 : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.Out.Write("start");
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.Out.Write("end");
        }
    }
    
    public class TestFilter3 : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                throw new Exception();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            Console.Out.Write("start");
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.Out.Write("end");
        }
    }
    
    public class ExceptionFilter1 : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Console.Out.Write("exception");
            context.Result = new EmptyResult();
        }
    }
    
    public class ExceptionFilter2 : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Console.Out.Write("exception");
        }
    }
}