using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.ExceptionFilters;

public class ExceptionHandledFilter2 : IExceptionFilter
{
    public Task OnException(IMessageContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        context.ExceptionHandled = true;
        TestStore.Stores.Add($"{nameof(ExceptionHandledFilter2)} {nameof(OnException)}");
        return Task.CompletedTask;
    }
}