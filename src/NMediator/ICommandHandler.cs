namespace NMediator
{
    public interface ICommandHandler<in TCommand> : IHandler<TCommand>
        where TCommand : class, ICommand
    {
    }
    
    public interface ICommandHandler<in TCommand, TResponse> : IHandler<TCommand, TResponse>
        where TCommand : class, ICommand
        where TResponse : class, IResponse
    {
    }
}