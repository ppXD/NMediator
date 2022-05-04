using NMediator.Context;

namespace NMediator.Filters;

public interface ICommandFilter : IExecutionFilter<ICommand, ICommandContext<ICommand>>
{
}

public interface ICommandFilter<TCommand> : IExecutionFilter<TCommand, ICommandContext<TCommand>> 
    where TCommand : class, ICommand
{
}