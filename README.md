<div align="center">

<p>

![logo][project-logo]

# NMediator

![build](https://github.com/ppXD/NMediator/workflows/build/badge.svg)
![test](https://github.com/ppXD/NMediator/workflows/test/badge.svg)
[![NuGet](https://img.shields.io/nuget/vpre/nmediator.svg)](https://www.nuget.org/packages/NMediator)

NMediator is a **simple and easy to use** mediator for .NET applications.

[Features](#features) •
[Installation](#installation) •
[Basic usage](#basic-usage) •
[License](#license)

</div>

## Overview
![screenshot][overview-screenshot]

## Features
- Async/await first
- No dependencies
- Fluent configuration
- In-process messaging
- Each message processing per scope
- Full coverage tests to ensure stability
- Filter pipeline behaviors similar to ASP.NET Core
- Built-in filters can be created to handle cross-cutting concerns
- Built-in dependency injection extensions include ASP.NET Core, Autofac, etc.

## Installation
Install [NMediator with NuGet](https://www.nuget.org/packages/NMediator):
```bash
Install-Package NMediator
```

## Basic usage
```csharp
using NMediator;

var mediator = new MediatorConfiguration()
    .RegisterHandlers(typeof(ExampleCommand).Assembly)
    .UseFilter<ExampleFilter>()
    .CreateMediator();

await mediator.SendAsync(new ExampleCommand());
await mediator.PublishAsync(new ExampleEvent());
var response = await mediator.SendAsync(new ExampleCommand());
var response = await mediator.RequestAsync(new ExampleRequest());
```

## Contract
NMediator has three kinds of messages contract:
- `ICommand`,`ICommand<out TResponse>` 

Command messages dispatched to a single handler. Send a command via the mediator:
```csharp
public class ExampleCommand : ICommand { }
public class ExampleHasResponseCommand : ICommand<ExampleResponse> { }

await mediator.SendAsync(new ExampleCommand());
var response = await mediator.SendAsync(new ExampleResponse());
```

- `IRequest<out TResponse>`

Request messages dispatched to a single handler. Send a request via the mediator:
```csharp
public class ExampleRequest : IRequest<ExampleResponse> { }

var response = await mediator.RequestAsync(new ExampleRequest());
```

- `IEvent`

Event messages dispatched to multi handlers. Publish a event via the mediator:
```csharp
public class ExampleEvent : IEvent { }

await mediator.PublishAsync(new ExampleEvent());
```

## Handler
Each message contract has a corresponding handler interface.
- `ICommandHandler<in TCommand>`,`ICommandHandler<in TCommand, TResponse>`
```csharp
public class ExampleCommandHandler : ICommandHandler<ExampleCommand> 
{
    public Task Handle(TestCommand command, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Command is {command}");
        return Task.CompletedTask;
    }
}
public class ExampleHasResponseCommandHandler : ICommandHandler<ExampleHasResponseCommand, ExampleResponse>
{
    public Task<ExampleResponse> Handle(ExampleHasResponseCommand command, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Command is {command}");
        return Task.FromResult(new ExampleResponse());
    }
}
```
- `IRequestHandler<in TRequest, TResponse>`
```csharp
public class ExampleRequestHandler : IRequestHandler<ExampleRequest, ExampleResponse>
{
    public Task<ExampleResponse> Handle(ExampleRequest request, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Request is {request}");
        return Task.FromResult(new ExampleResponse());
    }
}
```
- `IEventHandler<in TEvent>`
```csharp
public class ExampleEventHandler1 : IEventHandler<ExampleEvent> 
{
    public Task Handle(ExampleEvent @event, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Event is {@event}");
        return Task.CompletedTask;
    }
}
public class ExampleEventHandler2 : IEventHandler<ExampleEvent> 
{
    public Task Handle(ExampleEvent @event, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Event is {@event}");
        return Task.CompletedTask;
    }
}
```

## Filter

Filters in NMediator allow code to run before or after specific stages in the [message](#contract) processing pipeline.
It represents a similar pattern to filters in ASP.NET Core.

Custom filters can be created to handle cross-cutting concerns. Examples of cross-cutting concerns include error handling, caching, configuration, authorization, and logging.

**Filter types**

- `IHandlerFilter<in TMessage>`
- `IExceptionFilter`

**Handler filters**

NMediator provides various built-in handler filters:
- `IMessageFilter`, `IMessageFilter<in TMessage>`
- `ICommandFilter`, `ICommandFilter<in TCommand>`
- `IRequestFilter`, `IRequestFilter<in TRequest>`
- `IEventFilter`, `IEventFilter<in TEvent>`

Handler filters:
- Run immediately before and after a handler is called.
- Can change the arguments passed into a handler.
- Can change the result returned from the handler.

The following code shows a sample handler filters:
```csharp
public class GlobalMessagesFilter : IMessageFilter
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        // Do something before the handler executes.
    }
    public Task OnHandlerExecuted(IHandlerExecutedContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        // Do something before the handler executes.
    }
}

public class SpecifiedMessageFilter : IMessageFilter<ExampleMessage>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<ExampleMessage> context, CancellationToken cancellationToken = default)
    {
        // Do something before the handler executes.
    }
    public Task OnHandlerExecuted(IHandlerExecutedContext<ExampleMessage> context, CancellationToken cancellationToken = default)
    {
        // Do something before the handler executes.
    }
}
```

```csharp
public class GlobalCommandsFilter : ICommandFilter
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        // Do something before the command handler executes.
    }
    public Task OnHandlerExecuted(IHandlerExecutedContext<ICommand> context, CancellationToken cancellationToken = default)
    {
        // Do something before the command handler executes.
    }
}

public class SpecifiedCommandFilter : ICommandFilter<ExampleCommand>
{
    public Task OnHandlerExecuting(IHandlerExecutingContext<ExampleCommand> context, CancellationToken cancellationToken = default)
    {
        // Do something before the example command handler executes.
    }
    public Task OnHandlerExecuted(IHandlerExecutedContext<ExampleCommand> context, CancellationToken cancellationToken = default)
    {
        // Do something before the example handler handler executes.
    }
}
```

**Exception filters**

- Implement `IExceptionFilter`.
- Can be used to implement common error handling policies.
- Handle unhandled exceptions that occur in handler filters, or handle methods.

The following sample exception filter shows how to log the exception message:
```csharp
public class SampleExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;
    
    public SampleExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public Task OnException(IExceptionContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.LogError(context.Exception.ToString());
        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}
```

To handle an exception, set the `ExceptionHandled` property to true or assign the `Result` property. This stops propagation of the exception.

## IoC Container

NMediator uses `IDependencyScope` to instantiate handlers, middlewares, and filters.

No dependencies will use `DefaultDependencyScope` by default when `MediatorConfiguration` initialized.

**Recommended** to use the built-in dependency injection extensions.

[![NuGet](https://img.shields.io/badge/NMediator.Extensions-Autofac-brightgreen)](https://www.nuget.org/packages/NMediator.Extensions.Autofac)  
[![NuGet](https://img.shields.io/badge/NMediator.Extensions-Microsoft.DependencyInjection-brightgreen)](https://www.nuget.org/packages/NMediator.Extensions.Microsoft.DependencyInjection)

**ASP.NET Core (or Microsoft dependency injection)**
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddNMediator(typeof(Startup).Assembly);
    
    services.AddNMediator(config => 
    {
        config.UseFilter<ExampleFilter>();
        config.UseHandler<ExampleCommandHandler>();
        config.UseHandlers(typeof(ExampleCommandHandler));
        config.UseHandlers(typeof(Startup).Assembly, typeof(ExampleCommandHandler).Assembly);
    });
    
    services.AddNMediator(config => 
    {
        config.UseFilter<ExampleFilter>();
    }, typeof(Startup).Assembly, typeof(ExampleCommand).Assembly);
}
```

**Autofac**
```csharp
public void ConfigureContainer(ContainerBuilder builder)
{
    builder.RegisterNMediator(typeof(Startup).Assembly);
    
    builder.RegisterNMediator(config => 
    {
        config.UseFilter<ExampleFilter>();
        config.UseHandler<ExampleCommandHandler>();
        config.UseHandlers(typeof(ExampleCommandHandler));
        config.UseHandlers(typeof(Startup).Assembly, typeof(ExampleCommandHandler).Assembly);
    });
    
    builder.RegisterNMediator(config => 
    {
        config.UseFilter<ExampleFilter>();
    }, typeof(Startup).Assembly, typeof(ExampleCommand).Assembly);
}
```

**Other**

For more examples, check out the [complete examples][project-examples].

## License
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

[project-examples]: examples
[project-logo]: assets/logos/logo.png
[overview-screenshot]: assets/sceenshots/overview.png