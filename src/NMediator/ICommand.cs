namespace NMediator;

public interface ICommand : IMessage { }

public interface ICommand<out TResponse> : ICommand, IMessage<TResponse> { }