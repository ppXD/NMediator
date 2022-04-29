using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Responses;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestHasDerivedResponseCommandHandler1 : ICommandHandler<TestHasDerivedResponseCommand, TestDerivedResponse>
{
    public Task<TestDerivedResponse> Handle(ICommandContext<TestHasDerivedResponseCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestDerivedResponse());
    }
}

public class TestHasDerivedResponseCommandHandler2 : ICommandHandler<TestHasDerivedResponseCommand, TestResponse>
{
    public Task<TestResponse> Handle(ICommandContext<TestHasDerivedResponseCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestResponse());
    }
}

public class TestHasDerivedResponseCommandHandler3 : 
    ICommandHandler<TestHasDerivedResponseCommand, TestResponse>,
    ICommandHandler<TestHasDerivedResponseCommand, TestDerivedResponse>
{
    public Task<TestResponse> Handle(ICommandContext<TestHasDerivedResponseCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestResponse());
    }

    Task<TestDerivedResponse> IHandler<TestHasDerivedResponseCommand, TestDerivedResponse, ICommandContext<TestHasDerivedResponseCommand>>.Handle(ICommandContext<TestHasDerivedResponseCommand> context, CancellationToken cancellationToken)
    {
        TestStore.Stores.Add(context.Message);
        return Task.FromResult(new TestDerivedResponse());
    }
}