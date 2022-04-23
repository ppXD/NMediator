<div align="center">

# NMediator

![build](https://github.com/ppXD/NMediator/workflows/build/badge.svg)
![test](https://github.com/ppXD/NMediator/workflows/test/badge.svg)
[![NuGet](https://img.shields.io/nuget/vpre/nmediator.svg)](https://www.nuget.org/packages/NMediator)

NMediator is a **simple and easy to use** mediator for .NET applications.

[Getting started](#getting-started) •
[Installation](#installation) •
[Configuration](#configuration) •
[Integrations](#third-party-integrations)

</div>

## Overview
[![screenshot][product-screenshot]](https://example.com)

## Features
- Async/await first
- No dependencies
- In-process messaging
- Pipeline behaviors similar to ASP.NET Core
- Built-in filters can be created to handle cross-cutting concerns
- Built-in dependency injection extensions include ASP.NET Core, Autofac, etc.

## Installation
Install [NMediator with NuGet](https://www.nuget.org/packages/NMediator):

    Install-Package NMediator

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
var response = await mediator.SendAsync<ExampleCommand, ExampleResponse>(new ExampleCommand());
var response = await mediator.RequestAsync<ExampleRequest, ExampleResponse>(new ExampleRequest());
```

[product-screenshot]: assets/sceenshot/mediator.png