# NMediator

 NMediator is a simple, ultra-lightweight mediator for .NET applications.

## Basic usage

```csharp
var mediator = new MediatorConfiguration()
    .RegisterHandlers(typeof(this).Assembly)
    .CreateMediator();

await mediator.SendAsync(new TestCommmand());
await mediator.PublishAsync(new TestEvent());
await mediator.RequestAsync<TestRequest, TestResponse>(new TestRequest());
```
