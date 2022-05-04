using System;
using System.Collections.Generic;
using NMediator.Infrastructure;

namespace NMediator.Context;

public class CommandContext<TCommand> : MessageContext<TCommand>, ICommandContext<TCommand> where TCommand : ICommand
{
    public CommandContext(IMessageContext<TCommand> context) : base(context.Message, context.Scope, context.Filters, context.Handlers)
    {
    }
    
    public CommandContext(TCommand message, IDependencyScope scope, IEnumerable<Type> filters = null, IEnumerable<HandlerWrapper> handlers = null) : base(message, scope, filters, handlers)
    {
    }
}