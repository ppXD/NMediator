using System.Threading;
using System.Threading.Tasks;

namespace NMediator;

public interface ICommandHandler<in TCommand>
    where TCommand : class, ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : class, ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
}