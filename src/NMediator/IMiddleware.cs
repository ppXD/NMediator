using System.Threading.Tasks;

namespace NMediator
{
    public delegate Task MessageDelegate(object message);
    
    public interface IMiddleware
    {
        Task InvokeAsync(object message, MessageDelegate next);
    }
}