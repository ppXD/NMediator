using System;
using System.Linq;
using System.Threading.Tasks;
using NMediator.Test.TestData;
using Xunit;
using NMediator.Test.TestData.CommandHandlers;
using NMediator.Test.TestData.Commands;
using Shouldly;

namespace NMediator.Test.TestCommandHandlers
{
    public class BasicCommandHandlerShouldBeWork : TestBase
    {
        [Fact]
        public async Task Test()
        {
            var mediator = new MediatorConfiguration()
                .RegisterHandler<TestCommandHandler>()
                .CreateMediator();

            var command = new TestCommand(Guid.NewGuid());
            
            await mediator.SendAsync(command);
            
            TestStore.CommandStore.Count.ShouldBe(1);
            TestStore.CommandStore.Single().ShouldBe(command);
        }
    }
}