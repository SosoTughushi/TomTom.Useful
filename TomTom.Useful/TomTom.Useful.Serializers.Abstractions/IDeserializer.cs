using System;

namespace TomTom.Useful.Serializers.Abstractions
{
    public interface IDeserializer<T>
    {
        T Deserialize(byte[] data);
        T Deserialize(byte[] data, Type childType);
    }
}
