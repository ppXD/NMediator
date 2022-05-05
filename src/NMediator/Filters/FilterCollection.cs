using System;
using System.Collections.ObjectModel;

namespace NMediator.Filters;

public class FilterCollection : Collection<IFilter>
{
    public IFilter Add<TFilterType>() where TFilterType : IFilter
    {
        return Add(typeof(TFilterType));
    }
    
    public IFilter Add(Type filterType)
    {
        if (filterType == null)
            throw new ArgumentNullException(nameof(filterType));

        if (!typeof(IFilter).IsAssignableFrom(filterType))
            throw new ArgumentException($"{filterType.Name} does not implement IFilter", nameof(filterType));

        var filter = new TypeFilter(filterType);
        Add(filter);
        return filter;
    }
}