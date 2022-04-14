using NMediator.Context;

namespace NMediator.Filters;

public interface IRequestFilter : IExecutionFilter<IRequest, IRequestContext<IRequest>>
{
}

public interface IRequestFilter<TRequest> : IExecutionFilter<TRequest, IRequestContext<TRequest>> 
    where TRequest : class, IRequest
{
}