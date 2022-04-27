namespace NMediator.Extensions.DependencyInjection.Test.Messages.Commands;

public class BaseCommand : ICommand
{
}

public class BaseCommand<TResponse> : ICommand<TResponse>
{
}