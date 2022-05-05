using System.Threading;
using System.Threading.Tasks;
using NMediator.Filters;

namespace NMediator.Test.TestData.Filters.ExceptionFilters;

public class ExceptionUnHandledFilter1 : IExceptionFilter
{
    public Task OnException(ExceptionContext<IMessage> context, CancellationToken cancellationToken = default)
    {
        TestStore.Stores.Add($"{nameof(ExceptionUnHandledFilter1)} {nameof(OnException)}");
        return Task.CompletedTask;
    }
}