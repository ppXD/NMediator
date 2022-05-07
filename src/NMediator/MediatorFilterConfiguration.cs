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

        var matchedFilterTypes = new List<Type>
        {
            typeof(IMessageFilter), typeof(IMessageFilter<>).MakeGenericType(messageType), typeof(IExceptionFilter)
        };
        
        switch (messageType)
        {
            case not null when typeof(ICommand).IsAssignableFrom(messageType):
                matchedFilterTypes.AddRange(new[] { typeof(ICommandFilter), typeof(ICommandFilter<>).MakeGenericType(messageType) });
                break;
            case not null when typeof(IRequest).IsAssignableFrom(messageType):
                matchedFilterTypes.AddRange(new[] { typeof(IRequestFilter), typeof(IRequestFilter<>).MakeGenericType(messageType) });
                break;
            case not null when typeof(IEvent).IsAssignableFrom(messageType):
                matchedFilterTypes.AddRange(new[] { typeof(IEventFilter), typeof(IEventFilter<>).MakeGenericType(messageType) });
                break;
        }
        
        return Filters.Where(filter =>
        {
            var filterType = filter switch
            {
                TypeFilter typeFilter => typeFilter.ImplementationType,
                _ => filter.GetType()
            };
            return filterType.GetInterfaces().Any(i =>
                matchedFilterTypes.Any(m => i == m || i.IsGenericType && i.GetGenericTypeDefinition() == m));
        }).ToList();
    }
}