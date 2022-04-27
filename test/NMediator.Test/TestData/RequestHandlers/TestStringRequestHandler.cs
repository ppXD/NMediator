using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.RequestHandlers;

public class TestStringRequestHandler : IRequestHandler<TestStringRequest, string>
{
    public Task<string> Handle(IRequestContext<TestStringRequest> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult("Test string response");
    }
}