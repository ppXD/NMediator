namespace NMediator.Context;

public interface ICommandContext<out TCommand> : IMessageContext<TCommand> where TCommand : ICommand
{
}