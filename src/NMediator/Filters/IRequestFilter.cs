using NMediator.Context;

namespace NMediator.Filters;

public interface IRequestFilter : IExecutionFilter<IRequest, IMessageContext<IRequest>>
{
}

public interface IRequestFilter<TRequest> : IExecutionFilter<TRequest, IMessageContext<TRequest>> 
    where TRequest : class, IRequest
{
}