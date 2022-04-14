using System;
using NMediator.Test.TestData;

namespace NMediator.Test;

public class TestBase : IDisposable
{
    public void Dispose()
    {
        TestStore.Stores.Clear();
    }
}