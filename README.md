<div align="center">

<p align="center">
<a href="#">
</a>
<p align="center">
   <img src="assets/logos/logo.png" alt="Logo">
</p>

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
- `IRequest<out TResponse>`
- `IEvent`

## Handler

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
Middlewares are configured using `UseMiddleware` generic method.
```csharp
var configuration = new MediatorConfiguration();
configuration.UseMiddleware<ExampleMiddleware>();
configuration.UseMiddleware(typeof(ExampleMiddleware));
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