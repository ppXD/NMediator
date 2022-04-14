using NMediator.Context;

namespace NMediator;

public interface ICommandHandler<in TCommand> : IHandler<TCommand, ICommandContext<TCommand>>
    where TCommand : class, ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse> : IHandler<TCommand, TResponse, ICommandContext<TCommand>>
    where TCommand : class, ICommand
    where TResponse : class, IResponse
{
}