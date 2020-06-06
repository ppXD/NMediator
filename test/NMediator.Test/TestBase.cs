using System;
using NMediator.Test.TestData;
using Xunit;

namespace NMediator.Test
{
    [Collection("Mediator test")]
    public class TestBase
    {
        protected TestBase()
        {
            TestStore.CommandStore.Clear();
            TestStore.EventStore.Clear();
        }
    }
}