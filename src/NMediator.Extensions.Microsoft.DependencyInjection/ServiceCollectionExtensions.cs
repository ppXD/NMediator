using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

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
        
        config.HandlerConfiguration.GetHandlers().ToList().ForEach(f => services.AddTransient(f));
        config.PipelineConfiguration.Filters.ForEach(h => services.AddTransient(h));
        config.PipelineConfiguration.Middlewares.ForEach(m => services.AddTransient(m));
        
        return services;
    }
}