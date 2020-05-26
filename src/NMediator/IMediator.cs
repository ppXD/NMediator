using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public interface IMediator
    {
        Task SendAsync<TMessage>(TMessage command, CancellationToken cancellationToken = default)
            where TMessage : ICommand;
    }
}