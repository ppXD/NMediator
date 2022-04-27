namespace NMediator;

public interface IBasicCommand : IMessage { }

public interface IBasicCommand<out TResponse> : IBasicCommand, IMessage<TResponse> { }

public interface ICommand : IBasicCommand { }

public interface ICommand<out TResponse> : IBasicCommand<TResponse> { }