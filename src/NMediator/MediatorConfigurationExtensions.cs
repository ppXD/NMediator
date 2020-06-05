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
                                  t.GetTypeInfo().GetInterfaces().Any(type => type.IsHandlerInterface(typeof(IEventHandler<>))) || 
                                  t.GetTypeInfo().GetInterfaces().Any(type => type.IsHandlerInterface(typeof(IRequestHandler<,>))))
                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                foreach (var implementedInterface in handlerType.GetTypeInfo().ImplementedInterfaces)
                {
                    if (!IsHandlerInterface(implementedInterface, typeof(ICommandHandler<>)) &&
                        !IsHandlerInterface(implementedInterface, typeof(IEventHandler<>)) && 
                        !IsHandlerInterface(implementedInterface, typeof(IRequestHandler<,>))) continue;
                    
                    mediatorConfiguration.RegisterServices(sr =>
                    {
                        sr.Register(implementedInterface.GenericTypeArguments[0], handlerType);
                    });
                        
                    if (mediatorConfiguration.MessageBindings.ContainsKey(implementedInterface.GenericTypeArguments[0]))
                    {
                        mediatorConfiguration.MessageBindings[implementedInterface.GenericTypeArguments[0]].Add(handlerType);
                    }
                    else
                    {
                        mediatorConfiguration.MessageBindings.Add(implementedInterface.GenericTypeArguments[0], new List<Type> {handlerType});
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