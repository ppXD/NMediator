namespace NMediator;

public interface IMessage { }

public interface IMessage<out TResponse> : IMessage { }