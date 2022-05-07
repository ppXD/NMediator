using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TypeFilter = NMediator.Filters.TypeFilter;

namespace NMediator.Extensions.Microsoft.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNMediator(this IServiceCollection services, params Assembly[] assemblies)
        => AddNMediator(services, assemblies, null);
    
    public static IServiceCollection AddNMediator(this IServiceCollection services, Action<MediatorConfiguration> configuration) 
        => AddNMediator(services, null, configuration);

    public static IServiceCollection AddNMediator(this IServiceCollection services, Action<MediatorConfiguration> configuration, params Assembly[] assemblies)
        => AddNMediator(services, assemblies, configuration);
    
    public static IServiceCollection AddNMediator(this IServiceCollection services, Assembly[] assemblies, Action<MediatorConfiguration> configuration)
    {
        var config = new MediatorConfiguration();

        configuration?.Invoke(config);

        if (assemblies != null && assemblies.Any())
            config.RegisterHandlers(assemblies);

        services.AddSingleton(sp => config.UseDependencyScope(new MicrosoftDependencyScope(sp.CreateScope())));

        services.AddTransient<Mediator>();
        services.AddTransient<IMediator, Mediator>();

        foreach (var handler in config.HandlerConfiguration.GetHandlers())
        {
            services.AddTransient(handler);
        }

        foreach (var filter in config.FilterConfiguration.Filters)
        {
            if (filter is TypeFilter typeFilter)
                services.AddTransient(typeFilter.ImplementationType);
        }
        
        return services;
    }
}