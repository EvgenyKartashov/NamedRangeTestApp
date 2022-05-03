using Core.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Core.Extensions
{
    public static class MassTransitExtensions
    {
        public static IBusRegistrationConfigurator AddBusWithOptions(this IBusRegistrationConfigurator configurator, IConfiguration config)
        {
            var queueConfig = config.GetSection(QueueConfig.ConfigSection).Get<QueueConfig>();

            configurator.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(queueConfig.Host, queueConfig.VirtualHost, hostConfigurator =>
                {
                    hostConfigurator.Username(queueConfig.Username);
                    hostConfigurator.Password(queueConfig.Password);
                });
                cfg.ConfigureEndpoints(ctx);
            });

            configurator.SetKebabCaseEndpointNameFormatter();

            return configurator;
        }
    }
}
