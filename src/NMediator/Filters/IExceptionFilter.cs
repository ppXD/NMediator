using System.Threading;
using System.Threading.Tasks;

namespace NMediator.Filters;

public interface IExceptionFilter : IFilter
{
    Task OnException(IExceptionContext<IMessage> context, CancellationToken cancellationToken = default);
}