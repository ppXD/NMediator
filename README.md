# NMediator

 NMediator is a simple, ultra-lightweight mediator for .NET applications.

## Basic usage

```csharp
//Setup mediator
var mediator = new MediatorConfiguration()
    .RegisterHandlers(typeof(this).Assembly)
    .CreateMediator();

//Send command
await mediator.SendAsync(new TestCommmand());

//Publish event
await mediator.PublishAsync(new TestEvent());

//Send request and get response
var response = await mediator.RequestAsync<TestRequest, TestResponse>(new TestRequest());
```
