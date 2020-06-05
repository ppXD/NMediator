using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public interface IEventHandler<in TMessage>
        where TMessage : IEvent
    {
        Task Handle(TMessage @event, CancellationToken cancellationToken);
    }
}