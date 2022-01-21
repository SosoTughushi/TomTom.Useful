using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.CQRS
{
    public static class CqrsConfigurationExtensions
    {
        public static void AddCqrs(this IServiceCollection services, Action<CqrsConfigurationBuilder> configure)
        {
            var builder = new CqrsConfigurationBuilder(services);

            configure(builder);
        }

        public class CqrsConfigurationBuilder
        {
            private IServiceCollection services;

            public CqrsConfigurationBuilder(IServiceCollection services)
            {
                this.services = services;
            }

            public CqrsConfigurationBuilder ConvertMessagePublisherToCommandPublisher<TCommand>() where TCommand : ICommand
            {
                this.services.AddTransient(provider =>
                {
                    var bus = provider.GetService<IPublisher<TCommand>>()
                        ?? throw new InvalidOperationException($"IPublisher<{typeof(TCommand).Name}>  is not registered in DI.");

                    return bus.AsCommandPublisher();
                });

                return this;
            }
        }
    }


}
