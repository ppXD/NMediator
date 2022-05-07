using System;

namespace NMediator.Filters;

public class TypeFilter : IFilter
{
    public TypeFilter(Type type) =>
        ImplementationType = type ?? throw new ArgumentNullException(nameof(type));
    
    public Type ImplementationType { get; }
}