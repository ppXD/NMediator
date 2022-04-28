using System;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Events;
using NMediator.Test.TestData.Requests;
using Shouldly;
using Xunit;

namespace NMediator.Test;

public class MediatorFixture : TestBase
{
    [Fact]
    public void CannotSendNullMessage()
    {
        var mediator = new MediatorConfiguration().CreateMediator();
        
        var sendCommand1 = () => mediator.SendAsync(StaticMessage.TestCommand);
        var sendCommand2 = () => mediator.SendAsync(StaticMessage.InterfaceCommand);
        var sendRequest1 = () => mediator.RequestAsync<TestResponse>(StaticMessage.TestRequest);
        var sendRequest2 = () => mediator.RequestAsync<TestResponse>(StaticMessage.InterfaceRequest);
        var publishEvent1 = () => mediator.PublishAsync(StaticMessage.TestEvent);
        
        sendCommand1.ShouldThrow<ArgumentNullException>();
        sendCommand2.ShouldThrow<ArgumentNullException>();
        sendRequest1.ShouldThrow<ArgumentNullException>();
        sendRequest2.ShouldThrow<ArgumentNullException>();
        publishEvent1.ShouldThrow<ArgumentNullException>();
    }
}

public class StaticMessage
{
    public static TestCommand TestCommand;
    public static ITestCommand InterfaceCommand;

    public static TestRequest TestRequest;
    public static ITestRequest InterfaceRequest;
    
    public static TestEvent TestEvent;
}