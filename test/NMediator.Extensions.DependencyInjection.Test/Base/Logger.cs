using System.Collections.Generic;

namespace NMediator.Extensions.DependencyInjection.Test.Base;

public class Logger
{
    public IList<string> Messages { get; } = new List<string>();
}