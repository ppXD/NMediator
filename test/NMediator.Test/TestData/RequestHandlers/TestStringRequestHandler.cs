using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Requests;

namespace NMediator.Test.TestData.RequestHandlers;

public class TestStringRequestHandler : IRequestHandler<TestStringRequest, string>
{
    public Task<string> Handle(TestStringRequest request, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(request);
        return Task.FromResult("Test string response");
    }
}