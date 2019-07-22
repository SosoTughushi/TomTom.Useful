using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace TomTom.Useful.Repositories.Redis
{
    public static class RedisConfigurationExtensions
    {
        public static void AddRedisRepositories(this IServiceCollection collection, string configuration, Action<RedisRepositoryConfigurator> registerRepositories)
        {
            var config = ConfigurationOptions.Parse(configuration);
            collection.AddRedisRepositories(config, registerRepositories);
        }

        public static void AddRedisRepositories(this IServiceCollection collection, ConfigurationOptions configuration, Action<RedisRepositoryConfigurator> registerRepositories)
        {
            collection.AddSingleton(c => InitializeConnection(configuration));

            var builder = new RedisRepositoryConfigurator(collection);

            registerRepositories(builder);
        }

        private static IConnectionMultiplexer InitializeConnection(ConfigurationOptions config)
        {
            foreach (var endpoint in config.EndPoints)
            {
                var addressEndpoint = endpoint as DnsEndPoint;
                if (addressEndpoint == null)
                {
                    continue;
                }

                var port = addressEndpoint.Port;

                var isIp = IsIpAddress(addressEndpoint.Host);
                if (!isIp)
                {
                    //Please Don't use this line in blocking context. Please remove ".Result"
                    //Just for test purposes
                    IPHostEntry ip = Dns.GetHostEntryAsync(addressEndpoint.Host).Result;
                    config.EndPoints.Remove(addressEndpoint);
                    config.EndPoints.Add(ip.AddressList.First(), port);
                }
            }

            return ConnectionMultiplexer.Connect(config);
        }

        private static bool IsIpAddress(string host)
        {
            string ipPattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
            return Regex.IsMatch(host, ipPattern);
        }
    }
}
