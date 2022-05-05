using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.ExceptionFilters;

public class ExceptionHandledFilter1 : IExceptionFilter
{
    public Task OnException(ExceptionContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        context.ExceptionHandled = true;
        TestStore.Stores.Add($"{nameof(ExceptionHandledFilter1)} {nameof(OnException)}");
        return Task.CompletedTask;
    }
}