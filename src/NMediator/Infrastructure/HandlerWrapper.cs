using System;

namespace NMediator.Infrastructure;

public class HandlerWrapper : IEquatable<HandlerWrapper>
{
    public HandlerWrapper(Type handler, Type messageType, Type responseType)
    {
        Handler = handler;
        MessageType = messageType;
        ResponseType = responseType;
    }
    
    
    public Type Handler { get; }
    
    public Type MessageType { get; }

    public Type ResponseType { get; }

    public bool Equals(HandlerWrapper other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        
        return Handler == other.Handler && MessageType == other.MessageType && ResponseType == other.ResponseType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((HandlerWrapper)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Handler != null ? Handler.GetHashCode() : 0) * 397) ^ (MessageType != null ? MessageType.GetHashCode() : 0) ^ (ResponseType != null ? ResponseType.GetHashCode() : 0);
        }
    }

    public static bool operator ==(HandlerWrapper left, HandlerWrapper right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(HandlerWrapper left, HandlerWrapper right)
    {
        return !Equals(left, right);
    }
}