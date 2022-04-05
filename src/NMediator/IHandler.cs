using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public interface IHandler<in TMessage> 
        where TMessage : class, IMessage
    {
        Task Handle(TMessage message, CancellationToken cancellationToken = default);
    }
    
    public interface IHandler<in TMessage, TResponse> 
        where TMessage : class, IMessage
        where TResponse : class, IResponse
    {
        Task<TResponse> Handle(TMessage message, CancellationToken cancellationToken = default);
    }
}