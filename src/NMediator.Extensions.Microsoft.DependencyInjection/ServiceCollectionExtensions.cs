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

        if (assemblies != null && assemblies.Any())
        {
            config.RegisterHandlers(assemblies);
        }
        
        configuration?.Invoke(config);

        services.AddSingleton(sp => config.UseDependencyScope(new MicrosoftDependencyScope(sp.CreateScope())));
        
        services.AddTransient(sp => sp.GetRequiredService<MediatorConfiguration>().CreateMediator());
        services.AddTransient(sp => (Mediator) sp.GetRequiredService<MediatorConfiguration>().CreateMediator());
        
        config.Filters.Distinct().ToList().ForEach(f => services.AddTransient(f));
        config.Handlers.Distinct().ToList().ForEach(h => services.AddTransient(h));
        config.Middlewares.Distinct().ToList().ForEach(m => services.AddTransient(m));
        
        return services;
    }
}