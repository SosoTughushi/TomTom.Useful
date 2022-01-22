using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Demo.Domain.Dal;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Demo.Domain.Playlist.CommandHandlers;
using TomTom.Useful.EventSourcing;
using TomTom.Useful.Messaging.InMemory;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Domain
{
    public static class DemoDomainConfigurationExtensions
    {
        public static void AddDemoDomainCommandHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IHostedService, PlaylistAggregateCommandHandlers>();

            services.AddInMemoryBus(builder =>
            {
                builder.AddInMemoryMesageBus<Event<PlaylistIdentity>>(new InMemoryBusSettings
                {
                    LogWholeMessageOnFault = true
                });

                builder.AddInMemoryMesageBus<ResultOfPlaylistCommand>(new InMemoryBusSettings
                {
                    LogWholeMessageOnFault = false
                });
            });

            services.AddEventSourcing(builder =>
            {
                builder.ConvertMessagePublisherToEventPublisher<PlaylistIdentity>();
            });


            services.AddSingleton<PlaylistInMemoryRepository>();
            services.AddSingleton<IHostedService>(provider => provider.GetService<PlaylistInMemoryRepository>());
            services.AddTransient<IEntityByKeyProvider<PlaylistIdentity, Playlist.Playlist?>>(provider => provider.GetService<PlaylistInMemoryRepository>());
        }
    }
}
