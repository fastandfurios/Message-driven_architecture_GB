using MassTransit;
using MassTransit.Audit;
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
            var serviceProvider = services.BuildServiceProvider();
            var auditStore = serviceProvider.GetService<IMessageAuditStore>();

            services.AddMassTransit(config =>
            {
                config.AddConsumer<NotifyConsumer>(configurator =>
                    {
                        configurator.UseScheduledRedelivery(cfg =>
                        {
                            cfg.Interval(1, TimeSpan.FromSeconds(10));
                        });
                        configurator.UseMessageRetry(cfg =>
                        {
                            cfg.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                                intervalIncrement: TimeSpan.FromSeconds(2));
                        });
                    });

                config.AddConsumer<NotifyFaultConsumer>();

                config.AddDelayedMessageScheduler();

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseDelayedMessageScheduler();
                    cfg.UseInMemoryOutbox();
                    cfg.ConfigureEndpoints(registration: context);
                    cfg.ConnectSendAuditObservers(auditStore);
                    cfg.ConnectConsumeAuditObserver(auditStore);
                });
            });
        }
    }
}
