using System;

namespace NMediator.Test.TestData.Commands
{
    public class TestCommand : ICommand
    {
        public Guid Id { get; set; }

        public TestCommand(Guid id)
        {
            Id = id;
        }
    }
}