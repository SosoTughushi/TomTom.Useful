using System;

namespace TomTom.Useful.Serializers.Abstractions
{
    public interface ISerializer<T>
    {
        byte[] Serialize(T data);
        byte[] Serialize(T data, Type childType);
    }
}
