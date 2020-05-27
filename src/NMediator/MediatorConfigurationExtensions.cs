using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace NMediator
{
    public static class MediatorConfigurationExtensions
    {
        public static MediatorConfiguration RegisterHandlers(this MediatorConfiguration mediatorConfiguration, IEnumerable<Type> handlerTypes)
        {
            handlerTypes = handlerTypes
                .Where(x => !x.IsAbstract)
                .Where(t => t.GetTypeInfo().GetInterfaces().Any(type => type.IsHandlerInterface(typeof(ICommandHandler<>))) ||
                                  t.GetTypeInfo().GetInterfaces().Any(type => type.IsHandlerInterface(typeof(IEventHandler<>))))
                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                foreach (var implementedInterface in handlerType.GetTypeInfo().ImplementedInterfaces)
                {
                    if (IsHandlerInterface(implementedInterface, typeof(ICommandHandler<>)) ||
                        IsHandlerInterface(implementedInterface, typeof(IEventHandler<>)))
                    {
                        mediatorConfiguration.RegisterServices(sr =>
                        {
                            sr.Register(implementedInterface.GenericTypeArguments[0], handlerType);
                        });
                    }
                }
            }
            
            return mediatorConfiguration;
        }
        
        private static bool IsHandlerInterface(this Type type, Type handleType)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == handleType;
        }
    }
}