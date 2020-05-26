using System;

namespace NMediator.Ioc
{
    public interface IServiceResolver : IDisposable
    {
        T Resolve<T>();
        object Resolve(Type serviceType);
    }
}