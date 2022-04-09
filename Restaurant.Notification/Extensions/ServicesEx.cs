using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Notification.Consumers;

namespace Restaurant.Notification.Extensions
{
    internal static class ServicesEx
    {
        /// <summary> Adding and configuring mass transit. </summary>
        /// <param name="services">service collection interface</param>
        internal static void AddAndConfigMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<NotifyConsumer>(configurator =>
                    {
                        configurator.UseScheduledRedelivery(cfg =>
                        {
                            cfg.Intervals(TimeSpan.FromSeconds(10),
                                TimeSpan.FromSeconds(20),
                                TimeSpan.FromSeconds(30));
                        });
                        configurator.UseMessageRetry(cfg =>
                        {
                            cfg.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                                intervalIncrement: TimeSpan.FromSeconds(2));
                        });
                    })
                    .Endpoint(configure => configure.Temporary = true);

                config.AddConsumer<NotifyFaultConsumer>(configurator =>
                    {
                        configurator.UseScheduledRedelivery(cfg =>
                        {
                            cfg.Intervals(TimeSpan.FromSeconds(10),
                                TimeSpan.FromSeconds(20),
                                TimeSpan.FromSeconds(30));
                        });
                        configurator.UseMessageRetry(cfg =>
                        {
                            cfg.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                                intervalIncrement: TimeSpan.FromSeconds(2));
                        });
                    })
                    .Endpoint(configure => configure.Temporary = true);

                config.AddDelayedMessageScheduler();

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseDelayedMessageScheduler();
                    cfg.UseInMemoryOutbox();
                    cfg.ConfigureEndpoints(registration: context);
                });
            });
        }
    }
}
