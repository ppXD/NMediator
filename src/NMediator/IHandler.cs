using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator;

public interface IHandler<in TMessage, in TContext>
    where TMessage : class, IMessage
    where TContext : IMessageContext<TMessage>
{
    Task Handle(TContext context, CancellationToken cancellationToken = default);
}

public interface IHandler<in TMessage, TResponse, in TContext> 
    where TMessage : class, IMessage
    where TContext : IMessageContext<TMessage>
{
    Task<TResponse> Handle(TContext context, CancellationToken cancellationToken = default);
}