using System;
using System.Collections.Generic;
using NMediator.Ioc;

namespace NMediator.Context;

public class CommandContext<TCommand> : MessageContext<TCommand>, ICommandContext<TCommand> where TCommand : IMessage
{
    public CommandContext(IMessageContext<TCommand> context) : base(context.Message, context.Scope, context.ResponseType, context.Filters, context.MessageBindingHandlers)
    {
    }
    
    public CommandContext(TCommand message, IDependencyScope scope, Type responseType = null, IEnumerable<Type> filters = null, IEnumerable<Type> messageBindingHandlers = null) : base(message, scope, responseType, filters, messageBindingHandlers)
    {
    }
}