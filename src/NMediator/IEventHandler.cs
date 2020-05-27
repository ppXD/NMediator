namespace NMediator
{
    public interface IEventHandler<in TMessage>
        where TMessage : IEvent
    {
        
    }
}