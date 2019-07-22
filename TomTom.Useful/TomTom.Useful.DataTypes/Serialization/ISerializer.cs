using System;

namespace TomTom.Useful.DataTypes.Serialization
{
    public interface ISerializer<T>
    {
        byte[] Serialize(T data);
        byte[] Serialize(T data, Type childType);
    }
}
