using System.Threading;
using System.Threading.Tasks;

namespace NMediator
{
    public interface ICommandHandler<in TMessage>
        where TMessage : ICommand
    {
        Task Handle(TMessage command, CancellationToken cancellationToken);
    }
}