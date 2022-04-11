using NMediator.Context;

namespace NMediator.Filters;

public interface ICommandFilter : IExecutionFilter<ICommand, IMessageContext<ICommand>>
{
}

public interface ICommandFilter<TCommand> : IExecutionFilter<TCommand, IMessageContext<TCommand>> 
    where TCommand : class, ICommand
{
}