using System;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using NMediator.Extensions.Autofac;
using NMediator.Extensions.DependencyInjection.Test.Base;
using NMediator.Extensions.DependencyInjection.Test.Services;
using NMediator.Extensions.Microsoft.DependencyInjection;

namespace NMediator.Extensions.DependencyInjection.Test;

public class TestFixtureBase
{
    private readonly ContainerBuilder _builder;
    private readonly IServiceCollection _services;
    
    protected IContainer Container;
    protected IServiceProvider ServiceProvider;
    
    protected readonly Logger Logger;
    
    protected TestFixtureBase()
    {
        Logger = new Logger();
        _builder = new ContainerBuilder();
        _services = new ServiceCollection();
        
        _services.AddSingleton(Logger);
        _services.AddScoped<ILogService, LogService>();
        _services.AddScoped<IDoNothingService, DoNothingService>();

        _builder.RegisterInstance(Logger).SingleInstance();
        _builder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();
        _builder.RegisterType<DoNothingService>().As<IDoNothingService>().InstancePerLifetimeScope();
    }

    protected void RegisterMediator(DependencyInjectionType dependencyInjectionType, Action<MediatorConfiguration> configuration, Assembly[] assemblies, int way = 3)
    {
        switch (dependencyInjectionType)
        {
            case DependencyInjectionType.Autofac:
                switch (way)
                {
                    case 1:
                        _builder.RegisterNMediator(assemblies);
                        break;
                    case 2:
                        _builder.RegisterNMediator(configuration);
                        break;
                    case 3:
                        _builder.RegisterNMediator(configuration, assemblies);
                        break;
                }
                break;
            case DependencyInjectionType.MicrosoftDependencyInjection:
                switch (way)
                {
                    case 1:
                        _services.AddNMediator(assemblies);
                        break;
                    case 2:
                        _services.AddNMediator(configuration);
                        break;
                    case 3:
                        _services.AddNMediator(configuration, assemblies);
                        break;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dependencyInjectionType), dependencyInjectionType, null);
        }
    }
    
    protected TMediator GetMediator<TMediator>(DependencyInjectionType dependencyInjectionType)
        where TMediator : IMediator
    {
        Container ??= _builder.Build();
        ServiceProvider ??= _services.BuildServiceProvider();
        
        return dependencyInjectionType switch
        {
            DependencyInjectionType.Autofac => Container.Resolve<TMediator>(),
            DependencyInjectionType.MicrosoftDependencyInjection => ServiceProvider.GetRequiredService<TMediator>(),
            _ => throw new ArgumentOutOfRangeException(nameof(dependencyInjectionType), dependencyInjectionType, null)
        };
    }
}