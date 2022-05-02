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
- Pipeline behaviors similar to ASP.NET Core
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
    .UseMiddleware<ExampleMiddleware>()
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
    public Task Handle(ICommandContext<TestCommand> context, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Command is {context.Message}");
        return Task.CompletedTask;
    }
}
public class ExampleHasResponseCommandHandler : ICommandHandler<ExampleHasResponseCommand, ExampleResponse>
{
    public Task<ExampleResponse> Handle(ICommandContext<ExampleHasResponseCommand> context, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Command is {context.Message}");
        return Task.FromResult(new ExampleResponse());
    }
}
```
- `IRequestHandler<in TRequest, TResponse>`
```csharp
public class ExampleRequestHandler : IRequestHandler<ExampleRequest, ExampleResponse>
{
    public Task<ExampleResponse> Handle(IRequestContext<ExampleRequest> context, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Request is {context.Message}");
        return Task.FromResult(new ExampleResponse());
    }
}
```
- `IEventHandler<in TEvent>`
```csharp
public class ExampleEventHandler1 : IEventHandler<ExampleEvent> 
{
    public Task Handle(IEventContext<ExampleEvent> context, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Event is {context.Message}");
        return Task.CompletedTask;
    }
}
public class ExampleEventHandler2 : IEventHandler<ExampleEvent> 
{
    public Task Handle(IEventContext<ExampleEvent> context, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Event is {context.Message}");
        return Task.CompletedTask;
    }
}
```

## Middleware

NMediator middleware is assembled into pipeline to handle messages and responses.
- Automatically pass the message to the next component in the pipeline.
- Can perform work before and after the next component in the pipeline.

The NMediator pipeline consists of a sequence of middlewares, called one after the other. 
The [Overview](#overview) diagram demonstrates the concept.

To assemble the middleware into the pipeline you need to implement the `IMiddleware` interface.
The `IMiddleware` interface is defined as:
```csharp
public interface IMiddleware
{
    Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken);
    Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken);
}
```
`UseMiddleware<TMiddleware>` is the generic method to configure the middleware.
```csharp
var configuration = new MediatorConfiguration();
configuration.UseMiddleware<ExampleMiddleware>();
```

The following `LoggingMiddleware` example shows how to log elapsed time of each message:
```csharp
public class LoggingMiddleware : IMiddleware
{
    private readonly ILogger<LoggingMiddleware> _logger;
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    
    public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public Task OnExecuting(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _stopwatch.Start();
        _logger.LogInformation("Message starting");
        return Task.CompletedTask;
    }

    public Task OnExecuted(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Message finished in {_stopwatch.ElapsedMilliseconds}ms");
        return Task.CompletedTask;
    }
}
```

## Filter

## IoC Container

NMediator uses `IDependencyScope` to instantiate handlers, middlewares, and filters.

No dependencies will use `DefaultDependencyScope` by default when `MediatorConfiguration` initialized.

**Recommended** to use the built-in dependency injection extensions.

[![NuGet](https://img.shields.io/badge/NMediator.Extensions-Autofac-brightgreen)](https://www.nuget.org/packages/NMediator.Extensions.Autofac)  
[![NuGet](https://img.shields.io/badge/NMediator.Extensions-Microsoft.DependencyInjection-brightgreen)](https://www.nuget.org/packages/NMediator.Extensions.Microsoft.DependencyInjection)

[Complete examples][project-examples]

## License
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

[project-examples]: examples
[project-logo]: assets/logos/logo.png
[overview-screenshot]: assets/sceenshots/overview.png