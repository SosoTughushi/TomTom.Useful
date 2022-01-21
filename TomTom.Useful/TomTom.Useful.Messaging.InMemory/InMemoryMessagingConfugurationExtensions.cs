using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TomTom.Useful.Serializers.Json;

namespace TomTom.Useful.Messaging.InMemory
{
    public static class InMemoryMessagingConfugurationExtensions
    {
        public static void AddInMemoryBus(this IServiceCollection services, Action<InMemoryBusConfigurationBuilder> configure)
        {
            var builder = new InMemoryBusConfigurationBuilder(services);

            configure(builder);
        }

        public class InMemoryBusConfigurationBuilder
        {
            private IServiceCollection services;

            public InMemoryBusConfigurationBuilder(IServiceCollection services)
            {
                this.services = services;
            }

            public InMemoryBusConfigurationBuilder AddInMemoryMesageBus<TMessage>(InMemoryBusSettings settings) where TMessage : IMessage
            {
                services.AddSingleton(provider =>
                {
                    var logger = provider.GetService<ILogger<InMemoryMessageBus<TMessage>>>() 
                        ?? throw new InvalidOperationException($"ILogger<InMemoryBus<{typeof(TMessage).Name} is not registered in DI.");

                    var jsonSerializer = provider.GetService<JsonSerializer<TMessage>>()
                        ?? throw new InvalidOperationException($"ILogger<JsonSerializer<{typeof(TMessage).Name} is not registered in DI."); 

                    var bus = new InMemoryMessageBus<TMessage>(settings, logger, jsonSerializer);
                    return bus;
                });

                services.AddTransient<IPublisher<TMessage>>(provider => provider.GetService<InMemoryMessageBus<TMessage>>());
                services.AddTransient<ISubscriber<TMessage>>(provider => provider.GetService<InMemoryMessageBus<TMessage>>());

                return this;
            }
        }
    }
}
