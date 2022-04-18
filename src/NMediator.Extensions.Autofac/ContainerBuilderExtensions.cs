using Autofac;
using System.Reflection;
using NMediator.Ioc;

namespace NMediator.Extensions.Autofac;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterNMediator(this ContainerBuilder builder, params Assembly[] assemblies)
        => RegisterNMediator(builder, assemblies, null);
    
    public static ContainerBuilder RegisterNMediator(this ContainerBuilder builder, Action<MediatorConfiguration> configuration) 
        => RegisterNMediator(builder, null, configuration);

    public static ContainerBuilder RegisterNMediator(this ContainerBuilder builder, Action<MediatorConfiguration> configuration, params Assembly[] assemblies)
        => RegisterNMediator(builder, assemblies, configuration);
    
    public static ContainerBuilder RegisterNMediator(this ContainerBuilder builder, Assembly[] assemblies, Action<MediatorConfiguration> configuration)
    {
        var config = new MediatorConfiguration();

        if (assemblies != null && assemblies.Any())
            config.RegisterHandlers(assemblies);
        
        configuration?.Invoke(config);

        builder.RegisterType<AutofacDependencyScope>().AsImplementedInterfaces();
        builder.Register(c => config.UseDependencyScope(c.Resolve<IDependencyScope>())).SingleInstance();
        builder.Register(c => (Mediator) c.Resolve<MediatorConfiguration>().CreateMediator()).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        
        config.Filters.Distinct().ToList().ForEach(f => builder.RegisterType(f));
        config.Handlers.Distinct().ToList().ForEach(h => builder.RegisterType(h));
        config.Middlewares.Distinct().ToList().ForEach(m => builder.RegisterType(m));

        return builder;
    }
}