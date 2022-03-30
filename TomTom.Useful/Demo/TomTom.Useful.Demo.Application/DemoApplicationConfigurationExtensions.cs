using Microsoft.Extensions.DependencyInjection;
using TomTom.Useful.CQRS;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Messaging.InMemory;
using TomTom.Useful.Serializers.Json;
using TomTom.Useful.Demo.Domain;
using Microsoft.Extensions.Hosting;
using TomTom.Useful.Demo.Application.Projections;

namespace TomTom.Useful.Demo.Application
{
    public static class DemoApplicationConfigurationExtensions
    {
        public static void AddDemoApplication(this IServiceCollection services)
        {
            services.AddDemoDomainCommandHandlers();

            services.AddDemoAppProjections();
        }
    }
}
