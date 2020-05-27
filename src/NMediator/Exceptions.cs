using System;

namespace NMediator
{
    public class NoHandlerFoundException : Exception
    {
        public NoHandlerFoundException(Type messageType) : base($"No handler found for message type {messageType.FullName}")
        {
            
        }
    }

    public class MoreThanOneHandlerException : Exception
    {
        public MoreThanOneHandlerException(Type messageType)
            : base ($"Cannot have more than one handler for message type {messageType.FullName}")
        {
            
        }
    }
}