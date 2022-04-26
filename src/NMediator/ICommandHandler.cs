using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator;

public interface ICommandHandler<in TCommand>
    where TCommand : class, ICommand
{
    Task Handle(ICommandContext<TCommand> context, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : class, ICommand<TResponse>
{
    Task<TResponse> Handle(ICommandContext<TCommand> context, CancellationToken cancellationToken = default);
}