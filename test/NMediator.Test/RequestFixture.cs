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
        TestStore.Stores.Count.ShouldBe(4);
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