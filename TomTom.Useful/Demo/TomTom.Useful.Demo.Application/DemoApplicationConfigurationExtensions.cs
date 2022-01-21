using Microsoft.Extensions.DependencyInjection;
using TomTom.Useful.CQRS;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Messaging.InMemory;
using TomTom.Useful.Serializers.Json;
using TomTom.Useful.Demo.Domain;

namespace TomTom.Useful.Demo.Application
{
    public static class DemoApplicationConfigurationExtensions
    {
        public static void AddDemoApplication(this IServiceCollection services)
        {
            services.AddJsonSerialization();

            services.AddInMemoryBus(builder =>
            {
                builder.AddInMemoryMesageBus<ICommand<PlaylistIdentity>>(new InMemoryBusSettings
                {
                    LogWholeMessageOnFault = true
                });
            });

            services.AddCqrs(builder =>
            {
                builder.ConvertMessagePublisherToCommandPublisher<ICommand<PlaylistIdentity>>();
            });

            services.AddTransient<IPlaylistWriter, PlaylistWriter>();

            services.AddDemoDomainCommandHandlers();
        }
    }
}
