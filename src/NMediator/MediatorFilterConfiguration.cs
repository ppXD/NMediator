using System;
using System.Linq;
using System.Collections.Generic;
using NMediator.Filters;

namespace NMediator;

public class MediatorFilterConfiguration
{
    public FilterCollection Filters { get; } = new();
    
    protected internal void UseFilter(Type filter)
    {
        Filters.Add(filter);
    }
    
    protected internal void UseFilter<TFilterType>() where TFilterType : IFilter
    {
        Filters.Add<TFilterType>();
    }
    
    protected internal void UseFilter(IFilter filter)
    {
        Filters.Add(filter);
    }
    
    protected internal IList<IFilter> FindFilters(IMessage message)
    {
        var messageType = message.GetType();
        
        return Filters.Where(filter =>
        {
            var filterType = filter switch
            {
                TypeFilter typeFilter => typeFilter.ImplementationType, _ => filter.GetType()
            };
            return MatchHandlerFilter(filterType, messageType) || MatchExceptionFilter(filterType);
        }).ToList();
    }

    private static bool MatchHandlerFilter(Type filterType, Type messageType)
    {
        return filterType.GetInterfaces()
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandlerFilter<>) &&
                (x.GetGenericArguments()[0] == messageType || x.GetGenericArguments()[0].IsAssignableFrom(messageType)));
    }

    private static bool MatchExceptionFilter(Type filterType)
    {
        return filterType.GetInterfaces().Any(x => x == typeof(IExceptionFilter));
    }
}