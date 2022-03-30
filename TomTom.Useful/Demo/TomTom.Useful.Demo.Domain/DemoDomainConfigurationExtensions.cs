using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TomTom.Useful.Demo.Domain.Dal;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;
using TomTom.Useful.Messaging.InMemory;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Domain
{
    public static class DemoDomainConfigurationExtensions
    {
        public static void AddDemoDomainCommandHandlers(this IServiceCollection services)
        {
            services.AddInMemoryBus(builder =>
            {
                builder.AddInMemoryMesageBus<Event<PlaylistIdentity>>(new InMemoryBusSettings
                {
                    LogWholeMessageOnFault = true
                });
            });


            services.AddSingleton<PlaylistInMemoryRepository>();
            services.AddSingleton<IHostedService>(provider => provider.GetService<PlaylistInMemoryRepository>());
            services.AddSingleton<IFilteredListProvider<Playlist.Playlist>>(provider => provider.GetService<PlaylistInMemoryRepository>());
            services.AddTransient<IEntityByKeyProvider<PlaylistIdentity, Playlist.Playlist?>>(provider => provider.GetService<PlaylistInMemoryRepository>());
        }
    }
}
