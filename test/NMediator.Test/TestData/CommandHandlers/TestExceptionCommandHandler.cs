using System;
using System.Threading;
using System.Threading.Tasks;
using NMediator.Test.TestData.Commands;

namespace NMediator.Test.TestData.CommandHandlers;

public class TestExceptionCommandHandler : ICommandHandler<TestExceptionCommand>
{
    public Task Handle(TestExceptionCommand command, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add(command);
        throw new Exception("Test exception");
    }
}