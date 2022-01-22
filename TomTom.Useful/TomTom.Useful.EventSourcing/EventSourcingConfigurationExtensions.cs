using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.EventSourcing
{
    public static class EventSourcingConfigurationExtensions
    {

        public static void AddEventSourcing(this IServiceCollection services, Action<EventSourcingConfigurationBuilder> configure)
        {
            var builder = new EventSourcingConfigurationBuilder(services);

            configure(builder);
        }

        public class EventSourcingConfigurationBuilder
        {
            private IServiceCollection services;

            public EventSourcingConfigurationBuilder(IServiceCollection services)
            {
                this.services = services;
            }

            public EventSourcingConfigurationBuilder ConvertMessagePublisherToEventPublisher<TEventSourceIdentity>()
            {
                this.services.AddTransient(provider =>
                {
                    var bus = provider.GetService<IPublisher<Event<TEventSourceIdentity>>>()
                        ?? throw new InvalidOperationException($"IPublisher<{typeof(Event).Name}<{typeof(TEventSourceIdentity).Name}>> is not registered in DI.");

                    return bus.AsEventPublisher();
                });
                return this;
            }
        }
    }
}
