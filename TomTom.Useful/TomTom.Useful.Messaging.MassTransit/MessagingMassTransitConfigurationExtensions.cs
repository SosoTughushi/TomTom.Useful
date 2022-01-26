using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Messaging.MassTransit
{
    public static class MessagingMassTransitConfigurationExtensions
    {
        public static void AddMassTransitMessaging(this IServiceCollection services, Action<IServiceCollectionBusConfigurator> configure)
        {
            services.AddMassTransit(configurator =>
            {
                var decorated = new MassTransitCollectionBusConfiguratorDecorator(configurator); // :O Imposter
                configure(decorated);
            });
        }
    }
}
