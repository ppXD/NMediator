using System.Collections.Generic;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test.Base;

public class Logger
{
    public IList<string> Messages { get; } = new List<string>();
}