namespace NMediator.Ioc
{
    public interface IServiceRegistration
    {
        IServiceResolver CreateResolver();
    }
}