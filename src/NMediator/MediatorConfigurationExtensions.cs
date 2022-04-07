using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NMediator.Middleware;

namespace NMediator
{
    public static class MediatorConfigurationExtensions
    {
        public static MediatorConfiguration RegisterHandlers(this MediatorConfiguration mediatorConfiguration, IEnumerable<Type> handlerTypes)
        {
            RegisterHandlers(mediatorConfiguration, handlerTypes, typeof(ICommandHandler<>));
            RegisterHandlers(mediatorConfiguration, handlerTypes, typeof(ICommandHandler<,>));
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
                    
                    if (mediatorConfiguration.MessageHandlerBindings.ContainsKey(implementedInterface.GenericTypeArguments[0]))
                    {
                        mediatorConfiguration.MessageHandlerBindings[implementedInterface.GenericTypeArguments[0]].Add(handlerType);
                    }
                    else
                    {
                        mediatorConfiguration.MessageHandlerBindings.TryAdd(implementedInterface.GenericTypeArguments[0], new List<Type> {handlerType});
                    }
                }
            }
        }

        public static MediatorConfiguration RegisterMiddleware<TMiddleware>(this MediatorConfiguration mediatorConfiguration)
            where TMiddleware : class, IMiddleware
        {
            var processors = new MiddlewareProcessor(typeof(TMiddleware));

            if (mediatorConfiguration.MiddlewareProcessors.Any())
                mediatorConfiguration.MiddlewareProcessors.Last().Next = processors;
            mediatorConfiguration.MiddlewareProcessors.Add(processors);

            return mediatorConfiguration;
        }
        
        private static bool IsHandlerInterface(this Type type, Type handleType)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == handleType;
        }
    }
}