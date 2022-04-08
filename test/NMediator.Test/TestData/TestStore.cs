using System.Collections;
using System.Collections.Generic;

namespace NMediator.Test.TestData;

public static class TestStore
{
    public static readonly IList<ICommand> CommandStore = new List<ICommand>();
    public static readonly IList<IRequest> RequestStore = new List<IRequest>();
    public static readonly IList<IEvent> EventStore = new List<IEvent>();
}