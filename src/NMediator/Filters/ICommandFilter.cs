using NMediator.Context;

namespace NMediator.Filters;

public interface ICommandFilter : IExecutionFilter<IBasicCommand, ICommandContext<IBasicCommand>>
{
}

public interface ICommandFilter<TCommand> : IExecutionFilter<TCommand, ICommandContext<TCommand>> 
    where TCommand : class, IBasicCommand
{
}