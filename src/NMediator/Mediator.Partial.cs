using System;
using System.Collections.Generic;
using System.Linq;
using NMediator.Filters;

namespace NMediator;

public partial class Mediator
{
    private IEnumerable<Type> FindHandlerTypes<TMessage>(IEnumerable<Type> matchedHandlerTypes) where TMessage : class, IMessage
    {
        var messageType = typeof(TMessage);
        
        _messageHandlerBindings.TryGetValue(messageType, out var handlerTypes);

        if (handlerTypes == null)
            throw new NoHandlerFoundException(messageType);

        if (typeof(ICommand).IsAssignableFrom(typeof(TMessage)) && handlerTypes.Count > 1)
            throw new MoreThanOneHandlerException(typeof(TMessage));
        
        handlerTypes = handlerTypes.Where(handlerType =>
                handlerType.GetInterfaces()
                    .Any(i => matchedHandlerTypes.Any(m => i == m || i.GetGenericTypeDefinition() == m)))
            .ToList();
        
        if (!handlerTypes.Any())
            throw new NoHandlerFoundException(messageType);
        
        return handlerTypes;
    }

    private IEnumerable<Type> FindFilterTypes<TMessage>() where TMessage : class, IMessage
    {
        var messageType = typeof(TMessage);

        var matchedFilterTypes = new List<Type>
        {
            typeof(IMessageFilter), typeof(IMessageFilter<TMessage>), typeof(IExceptionFilter)
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
        
        var filters = _filters.Where(f =>
            f.GetInterfaces().Any(i => matchedFilterTypes.Any(m => i == m || i.IsGenericType && i.GetGenericTypeDefinition() == m)));

        return filters;
    }
}
