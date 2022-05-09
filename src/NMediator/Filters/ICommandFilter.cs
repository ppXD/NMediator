namespace NMediator.Filters;

public interface ICommandFilter : IMessageFilter<ICommand>
{
}

public interface ICommandFilter<in TCommand> : IMessageFilter<TCommand> 
    where TCommand : class, ICommand
{
}