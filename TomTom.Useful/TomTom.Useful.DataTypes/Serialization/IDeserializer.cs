using System;

namespace TomTom.Useful.DataTypes.Serialization
{
    public interface IDeserializer<T>
    {
        T Deserialize(byte[] data);
        T Deserialize(byte[] data, Type childType);
    }
}
