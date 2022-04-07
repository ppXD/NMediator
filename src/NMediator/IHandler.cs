using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator
{
    public interface IHandler<in TMessage> 
        where TMessage : class, IMessage
    {
        Task Handle(IMessageContext<TMessage> context, CancellationToken cancellationToken = default);
    }
    
    public interface IHandler<in TMessage, TResponse> 
        where TMessage : class, IMessage
        where TResponse : class, IResponse
    {
        Task<TResponse> Handle(IMessageContext<TMessage> context, CancellationToken cancellationToken = default);
    }
}