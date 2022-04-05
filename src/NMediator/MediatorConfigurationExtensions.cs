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
            RegisterHandlers(mediatorConfiguration, handlerTypes, typeof(ICommandHandler<>));
            RegisterHandlers(mediatorConfiguration, handlerTypes, typeof(IRequestHandler<,>));
            RegisterHandlers(mediatorConfiguration, handlerTypes, typeof(IEventHandler<>));
            
            return mediatorConfiguration;
        }

        private static void RegisterHandlers(MediatorConfiguration mediatorConfiguration, IEnumerable<Type> handlerTypes, Type targetHandlerType)
        {
            var handlers = handlerTypes
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsClass && x.GetInterfaces()
                .Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == targetHandlerType)).ToList();
            
            foreach (var handlerType in handlers)
            {
                foreach (var implementedInterface in handlerType.GetTypeInfo().ImplementedInterfaces)
                {
                    if (!IsHandlerInterface(implementedInterface, targetHandlerType)) continue;
                    
                    if (mediatorConfiguration.MessageBindings.ContainsKey(implementedInterface.GenericTypeArguments[0]))
                    {
                        mediatorConfiguration.MessageBindings[implementedInterface.GenericTypeArguments[0]].Add(handlerType);
                    }
                    else
                    {
                        mediatorConfiguration.MessageBindings.TryAdd(implementedInterface.GenericTypeArguments[0], new List<Type> {handlerType});
                    }
                }
            }
        }
        
        private static bool IsHandlerInterface(this Type type, Type handleType)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == handleType;
        }
    }
}