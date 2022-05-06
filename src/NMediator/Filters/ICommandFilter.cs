namespace NMediator.Filters;

public interface ICommandFilter : IHandlerFilter<ICommand>
{
}

public interface ICommandFilter<in TCommand> : IHandlerFilter<TCommand> 
    where TCommand : class, ICommand
{
}