using Autofac;
using System.Reflection;
using TypeFilter = NMediator.Filters.TypeFilter;

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

        configuration?.Invoke(config);

        if (assemblies != null && assemblies.Any())
            config.RegisterHandlers(assemblies);
        
        builder.RegisterType<AutofacDependencyScope>().AsImplementedInterfaces();
        builder.Register(c => config.UseDependencyScope(c.Resolve<IDependencyScope>())).SingleInstance();
        
        builder.RegisterType<Mediator>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        
        foreach (var handler in config.HandlerConfiguration.GetHandlers())
        {
            builder.RegisterType(handler);
        }

        foreach (var filter in config.FilterConfiguration.Filters)
        {
            if (filter is TypeFilter typeFilter)
                builder.RegisterType(typeFilter.ImplementationType);
        }

        return builder;
    }
}