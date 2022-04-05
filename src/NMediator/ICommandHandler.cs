namespace NMediator
{
    public interface ICommandHandler<in TMessage> : IHandler<TMessage>
        where TMessage : class, ICommand
    {
    }
}