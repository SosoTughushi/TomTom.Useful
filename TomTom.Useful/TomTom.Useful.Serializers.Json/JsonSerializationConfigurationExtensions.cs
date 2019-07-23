using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TomTom.Useful.Serializers.Abstractions;

namespace TomTom.Useful.Serializers.Json
{
    public static class JsonSerializationConfigurationExtensions
    {
        public static void AddJsonSerialization(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ISerializer<>), typeof(JsonSerializer<>));
            services.AddSingleton(typeof(IDeserializer<>), typeof(JsonSerializer<>));
        }
    }
}
