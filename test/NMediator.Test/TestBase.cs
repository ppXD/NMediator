using System;
using NMediator.Test.TestData;

namespace NMediator.Test;

public class TestBase : IDisposable
{
    public void Dispose()
    {
        TestStore.CommandStore.Clear();
        TestStore.EventStore.Clear();
        TestStore.RequestStore.Clear();
    }
}