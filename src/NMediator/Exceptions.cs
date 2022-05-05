using System;

namespace NMediator;

public class NoHandlerFoundException : Exception
{
    public NoHandlerFoundException(Type messageType) : base($"No handler found for message type {messageType.FullName}")
    {
            
    }
}