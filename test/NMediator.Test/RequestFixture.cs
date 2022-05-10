using System;
using System.Threading.Tasks;
using NMediator.Test.TestData;
using NMediator.Test.TestData.RequestHandlers;
using NMediator.Test.TestData.Requests;
using NMediator.Test.TestData.Responses;
using Shouldly;
using Xunit;

namespace NMediator.Test;

public class RequestFixture : TestBase
{
    [Fact]
    public async Task ShouldRequestHandled()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestRequestHandler>()
            .RegisterHandler<TestStringRequestHandler>()
            .CreateMediator();

        var request = new TestRequest();

        var response1 = await mediator.RequestAsync<TestResponse>(request);
        var response2 = await mediator.RequestAsync(new TestStringRequest());
        
        response1.ShouldNotBeNull();
        response2.ShouldNotBeEmpty();
        
        TestStore.Stores.Count.ShouldBe(2);
    }
        
    [Fact]
    public async Task ShouldRequestSendToItsHandler()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestRequestHandler>()
            .RegisterHandler<TestOtherRequestHandler>()
            .RegisterHandler<TestRequestButOtherResponseHandler>()
            .CreateMediator();

        var request = new TestRequest();

        var response1 = await mediator.RequestAsync<TestResponse>(request);
        var response2 = await mediator.RequestAsync<TestOtherResponse>(request);
        
        response1.ShouldNotBeNull();
        response2.ShouldNotBeNull();
        response1.Result.ShouldBe("Test response");
        response2.Result.ShouldBe("Test request but other response");

        TestStore.Stores.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldOneHandlerToHandleMultipleRequests()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestRequestAllInOneHandler>()
            .RegisterHandler<TestOtherRequestAllInOneHandler>()
            .CreateMediator();

        var request = new TestRequest();
        var otherRequest = new TestOtherRequest();
        
        var response1 = await mediator.RequestAsync<TestResponse>(request);
        var response2 = await mediator.RequestAsync<TestOtherResponse>(request);
        var response3 = await mediator.RequestAsync<TestResponse>(otherRequest);
        var response4 = await mediator.RequestAsync<TestOtherResponse>(otherRequest);
        
        response1.ShouldNotBeNull();
        response2.ShouldNotBeNull();
        response3.ShouldNotBeNull();
        response4.ShouldNotBeNull();
        
        response1.Result.ShouldBe("Test response for test request");
        response2.Result.ShouldBe("Test other response for test request");
        response3.Result.ShouldBe("Test response for test other request");
        response4.Result.ShouldBe("Test other response for test other request");

        TestStore.Stores.Count.ShouldBe(4);
    }
    
    [Fact]
    public void ShouldNotHandleUnMatchedResponse()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestRequestHandler>()
            .CreateMediator();

        var funcTask = () => mediator.RequestAsync<TestOtherResponse>(new TestRequest());

        funcTask.ShouldThrow<NoHandlerFoundException>();
    }
    
    [Fact]
    public async Task ShouldDerivedResponseCommandBeWork()
    {
        var mediator1 = new MediatorConfiguration()
            .RegisterHandler<TestDerivedRequestHandler1>()
            .CreateMediator();
        
        var mediator2 = new MediatorConfiguration()
            .RegisterHandler<TestDerivedRequestHandler2>()
            .CreateMediator();
        
        var mediator3 = new MediatorConfiguration()
            .RegisterHandler<TestDerivedRequestHandler3>()
            .CreateMediator();
        
        var response1 = await mediator1.RequestAsync(new TestDerivedRequest());
        var response2 = await mediator1.RequestAsync<TestResponse>(new TestDerivedRequest());

        var response3 = () => mediator2.RequestAsync(new TestDerivedRequest());
        var response4 = await mediator2.RequestAsync<TestResponse>(new TestDerivedRequest());
        
        var response5 = () => mediator3.RequestAsync(new TestDerivedRequest());
        var response6 = await mediator3.RequestAsync<TestResponse>(new TestDerivedRequest());
        
        response1.ShouldNotBeNull();
        response2.ShouldNotBeNull();
        response3.ShouldThrow<NoHandlerFoundException>();
        response4.ShouldNotBeNull();
        response5.ShouldThrow<Exception>();
        response6.ShouldNotBeNull();
        TestStore.Stores.Count.ShouldBe(5);
    }
    
    [Fact]
    public async Task ShouldInheritHandlerBeWork()
    {
        var mediator1 = new MediatorConfiguration()
            .RegisterHandler<ITestRequestHandler>()
            .CreateMediator();

        var mediator2 = new MediatorConfiguration()
            .RegisterHandler<ITestRequestHandler>()
            .RegisterHandler<TestInterfaceRequestHandler>()
            .CreateMediator();
        
        var mediator3 = new MediatorConfiguration()
            .RegisterHandler<ITestRequestHandler>()
            .RegisterHandler<TestOneWayRequestHandler>()
            .CreateMediator();
        
        var mediator4 = new MediatorConfiguration()
            .RegisterHandler<ITestRequestHandler>()
            .RegisterHandler<TestOneWayRequestHandler>()
            .RegisterHandler<TestTwoWayRequestHandler>()
            .CreateMediator();
        
        var mediator5 = new MediatorConfiguration()
            .RegisterHandler<ITestRequestHandler>()
            .RegisterHandler<TestOneWayRequestHandler>()
            .RegisterHandler<TestTwoWayRequestHandler>()
            .RegisterHandler<TestAllWayRequestHandler>()
            .CreateMediator();
        
        var mediator6 = new MediatorConfiguration()
            .RegisterHandler<ITestRequestHandler>()
            .RegisterHandler<TestOneWayRequestHandler>()
            .RegisterHandler<TestTwoWayRequestHandler>()
            .RegisterHandler<TestAllWayRequestHandler>()
            .RegisterHandler<TestInheritAllWayRequestHandler>()
            .CreateMediator();
        
        await mediator1.RequestAsync<TestResponse>(new TestInterfaceRequest());
        await mediator1.RequestAsync<TestDerivedResponse>(new TestInterfaceRequest());
        await mediator2.RequestAsync<TestResponse>(new TestInterfaceRequest());
        await mediator2.RequestAsync<TestDerivedResponse>(new TestInterfaceRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(TestInterfaceRequestHandler)}");
        TestStore.Stores[3].ShouldBe($"{nameof(TestInterfaceRequestHandler)}");
        TestStore.Stores.Clear();
        
        await mediator1.RequestAsync<TestResponse>(new TestOneWayRequest());
        await mediator1.RequestAsync<TestDerivedResponse>(new TestOneWayRequest());
        await mediator3.RequestAsync<TestResponse>(new TestOneWayRequest());
        await mediator3.RequestAsync<TestDerivedResponse>(new TestOneWayRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(TestOneWayRequestHandler)}");
        TestStore.Stores[3].ShouldBe($"{nameof(TestOneWayRequestHandler)}");
        TestStore.Stores.Clear();
        
        await mediator1.RequestAsync<TestResponse>(new TestTwoWayRequest());
        await mediator1.RequestAsync<TestDerivedResponse>(new TestTwoWayRequest());
        await mediator4.RequestAsync<TestResponse>(new TestTwoWayRequest());
        await mediator4.RequestAsync<TestDerivedResponse>(new TestTwoWayRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(TestTwoWayRequestHandler)}");
        TestStore.Stores[3].ShouldBe($"{nameof(TestTwoWayRequestHandler)}");
        TestStore.Stores.Clear();
        
        await mediator1.RequestAsync<TestResponse>(new TestInheritAllWayRequest());
        await mediator1.RequestAsync<TestDerivedResponse>(new TestInheritAllWayRequest());
        await mediator5.RequestAsync<TestResponse>(new TestInheritAllWayRequest());
        await mediator5.RequestAsync<TestDerivedResponse>(new TestInheritAllWayRequest());
        await mediator6.RequestAsync<TestResponse>(new TestInheritAllWayRequest()); 
        await mediator6.RequestAsync<TestDerivedResponse>(new TestInheritAllWayRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestRequestHandler)}");
        TestStore.Stores[2].ShouldBe($"{nameof(TestAllWayRequestHandler)}");
        TestStore.Stores[3].ShouldBe($"{nameof(TestAllWayRequestHandler)}");
        TestStore.Stores[4].ShouldBe($"{nameof(TestInheritAllWayRequestHandler)}");
        TestStore.Stores[5].ShouldBe($"{nameof(TestInheritAllWayRequestHandler)}");
    }

    [Fact]
    public async Task ShouldAbstractHandlerBeWork()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<ITestAbstractRequestAsITestResponseHandler>()
            .CreateMediator();
        
        var mediator1 = new MediatorConfiguration()
            .RegisterHandler<ITestAbstractRequestAsTestResponseHandler>()
            .RegisterHandler<ITestAbstractRequestAsDerivedResponseHandler>()
            .CreateMediator();
        
        var mediator2 = new MediatorConfiguration()
            .RegisterHandler<ITestAbstractRequestAsTestResponseHandler>()
            .RegisterHandler<ITestAbstractRequestAsDerivedResponseHandler>()
            .RegisterHandler<TestAbstractRequestBaseAsTestResponseHandler>()
            .RegisterHandler<TestAbstractRequestBaseAsDerivedResponseHandler>()
            .CreateMediator();
        
        var mediator3 = new MediatorConfiguration()
            .RegisterHandler<ITestAbstractRequestAsTestResponseHandler>()
            .RegisterHandler<ITestAbstractRequestAsDerivedResponseHandler>()
            .RegisterHandler<TestAbstractRequestBaseAsTestResponseHandler>()
            .RegisterHandler<TestAbstractRequestBaseAsDerivedResponseHandler>()
            .RegisterHandler<TestAbstractRequestAsTestResponseHandler>()
            .RegisterHandler<TestAbstractRequestAsDerivedResponseHandler>()
            .CreateMediator();
        
        await mediator.RequestAsync<TestResponse>(new TestAbstractRequest());
        await mediator.RequestAsync<TestDerivedResponse>(new TestAbstractRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestAbstractRequestAsITestResponseHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestAbstractRequestAsITestResponseHandler)}");
        TestStore.Stores.Clear();
        
        await mediator1.RequestAsync<TestResponse>(new TestAbstractRequest());
        await mediator1.RequestAsync<TestDerivedResponse>(new TestAbstractRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(ITestAbstractRequestAsTestResponseHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(ITestAbstractRequestAsDerivedResponseHandler)}");
        TestStore.Stores.Clear();
        
        await mediator2.RequestAsync<TestResponse>(new TestAbstractRequest());
        await mediator2.RequestAsync<TestDerivedResponse>(new TestAbstractRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(TestAbstractRequestBaseAsTestResponseHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestAbstractRequestBaseAsDerivedResponseHandler)}");
        TestStore.Stores.Clear();
        
        await mediator3.RequestAsync<TestResponse>(new TestAbstractRequest());
        await mediator3.RequestAsync<TestDerivedResponse>(new TestAbstractRequest());
        
        TestStore.Stores[0].ShouldBe($"{nameof(TestAbstractRequestAsTestResponseHandler)}");
        TestStore.Stores[1].ShouldBe($"{nameof(TestAbstractRequestAsDerivedResponseHandler)}");
    }
    
    [Fact]
    public async Task DuplicatedHandlerShouldNotThrow()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestRequestHandler>()
            .RegisterHandler<TestRequestDuplicatedHandler>()
            .CreateMediator();
        
        var response = await mediator.RequestAsync<TestResponse>(new TestRequest());

        response.ShouldNotBeNull();
        response.Result.ShouldBe("Test response");
    }
}