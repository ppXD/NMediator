

namespace NMediator.Filters;

public interface ICommandFilter
{
}

public interface ICommandFilter<TCommand>
    where TCommand : class, ICommand
{
}