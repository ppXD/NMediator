using System.Threading;
using System.Threading.Tasks;

namespace NMediator;

public interface IEventHandler<in TEvent>
    where TEvent : class, IEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken = default);
}