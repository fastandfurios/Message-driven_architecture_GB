﻿using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Kitchen.Consumers;
using Restaurant.Kitchen.Exceptions;

namespace Restaurant.Kitchen.Extensions
{
    internal static class ServicesEx
    {
        /// <summary> Adding and configuring mass transit. </summary>
        /// <param name="services">service collection interface</param>
        internal static void AddAndConfigMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<KitchenTableBookedConsumer>(cfg =>
                {
                    cfg.UseScheduledRedelivery(config =>
                    {
                        config.Intervals(TimeSpan.FromSeconds(10),
                            TimeSpan.FromSeconds(20),
                            TimeSpan.FromSeconds(30));
                    });
                    cfg.UseMessageRetry(config =>
                    {
                        config.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                            intervalIncrement: TimeSpan.FromSeconds(2));
                        config.Handle<LasagnaException>();
                    });
                });

                configure.AddConsumer<KitchenFaultConsumer>();

                configure.AddDelayedMessageScheduler();

                configure.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseDelayedMessageScheduler();
                    cfg.UseInMemoryOutbox();
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
