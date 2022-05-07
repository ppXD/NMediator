namespace NMediator.Filters;

public interface IRequestFilter : IHandlerFilter<IRequest>
{
}

public interface IRequestFilter<TRequest> : IHandlerFilter<TRequest>
    where TRequest : class, IRequest
{
}