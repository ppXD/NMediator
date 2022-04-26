using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator;

public interface IEventHandler<in TEvent>
    where TEvent : class, IEvent
{
    Task Handle(IEventContext<TEvent> context, CancellationToken cancellationToken = default);
}