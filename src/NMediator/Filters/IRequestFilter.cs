namespace NMediator.Filters;

public interface IRequestFilter : IMessageFilter<IRequest>
{
}

public interface IRequestFilter<in TRequest> : IMessageFilter<TRequest>
    where TRequest : class, IRequest
{
}