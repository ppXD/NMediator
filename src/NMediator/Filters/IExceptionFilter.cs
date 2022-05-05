using System.Threading;
using System.Threading.Tasks;

namespace NMediator.Filters;

public interface IExceptionFilter : IFilter
{
    Task OnException(ExceptionContext<IMessage> context, CancellationToken cancellationToken = default);
}