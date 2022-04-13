using System;
using System.Threading.Tasks;
using NMediator.Test.TestData.CommandHandlers;
using NMediator.Test.TestData.Commands;
using NMediator.Test.TestData.Filters;
using Xunit;

namespace NMediator.Test;

public class FilterFixture : TestBase
{
    [Fact]
    public async Task ShouldFilterInvoked()
    {
        var mediator = new MediatorConfiguration()
            .RegisterHandler<TestCommandHandler>()
            .UseFilter<TestFirstMessageFilter>()
            .UseFilter<TestSecondMessageFilter>()
            .UseFilter<TestCommandMessageFilter>()
            .CreateMediator();

        var command = new TestCommand(Guid.NewGuid());
        
        await mediator.SendAsync(command);
    }
}