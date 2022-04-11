using System.Threading;
using System.Threading.Tasks;
using NMediator.Context;

namespace NMediator.Filters;

public interface IExceptionFilter : IFilter
{
    Task OnException(IMessageContext<IMessage> context, CancellationToken cancellationToken = default);
}