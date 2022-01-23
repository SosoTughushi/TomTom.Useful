using Microsoft.Extensions.DependencyInjection;
using TomTom.Useful.CQRS;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Messaging.InMemory;
using TomTom.Useful.Serializers.Json;
using TomTom.Useful.Demo.Domain;
using Microsoft.Extensions.Hosting;
using TomTom.Useful.Demo.Application.Playlist;
using TomTom.Useful.Demo.Application.Projections;

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

            services.AddSingleton<PlaylistWriter>();
            services.AddSingleton<IPlaylistWriter>(provider => provider.GetService<PlaylistWriter>());
            services.AddSingleton<IHostedService>(provider => provider.GetService<PlaylistWriter>());
            services.AddDemoDomainCommandHandlers();

            services.AddTransient<IPlaylistReader, PlaylistReader>();
            services.AddDemoAppProjections();
        }
    }
}
