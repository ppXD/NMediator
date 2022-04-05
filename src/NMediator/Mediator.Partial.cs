using System;
using System.Collections.Generic;
using System.Linq;

namespace NMediator
{
    public partial class Mediator
    {
        private List<Type> FindHandlerTypes(Type messageType)
        {
            Configuration.MessageBindings.TryGetValue(messageType, out var handlerTypes);

            if (handlerTypes == null || !handlerTypes.Any())
                throw new NoHandlerFoundException(messageType);

            return handlerTypes;
        }
    }
}