using System;
using System.Collections.Generic;

namespace NMediator.Test.TestData;

public static class TestStore
{
    [ThreadStatic] private static IList<object> _stores;

    public static IList<object> Stores => _stores ??= new List<object>();
}