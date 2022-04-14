using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestExceptionCommandHandler : ICommandHandler<TestExceptionCommand>
{
    public Task Handle(ICommandContext<TestExceptionCommand> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(context.Message);
        throw new Exception("Test exception");
    }
}