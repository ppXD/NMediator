namespace NMediator;

public interface IRequest : IMessage { }

public interface IRequest<out TResponse> : IRequest, IMessage<TResponse> { }