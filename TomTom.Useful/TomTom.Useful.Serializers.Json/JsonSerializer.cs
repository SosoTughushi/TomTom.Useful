using Newtonsoft.Json;
using System;
using TomTom.Useful.Serializers.Abstractions;

namespace TomTom.Useful.Serializers.Json
{
    public class JsonSerializer<T> : ISerializer<T>, IDeserializer<T>
    {
        public T Deserialize(byte[] data)
        {
            var str = System.Text.Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(str);
        }

        public T Deserialize(byte[] data, Type childType)
        {
            var str = System.Text.Encoding.UTF8.GetString(data);
            return (T)JsonConvert.DeserializeObject(str, childType);
        }

        public string SerializeToString(T data)
        {
            var str = JsonConvert.SerializeObject(data);
            return str;
        }

        public byte[] Serialize(T data)
        {
            var str = SerializeToString(data);
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        public byte[] Serialize(T data, Type childType)
        {
            var str = JsonConvert.SerializeObject(data);

            return System.Text.Encoding.UTF8.GetBytes(str);
        }
    }
}
