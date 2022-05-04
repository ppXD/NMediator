namespace NMediator.Filters;

public interface IRequestFilter
{
}

public interface IRequestFilter<TRequest>
    where TRequest : class, IRequest
{
}