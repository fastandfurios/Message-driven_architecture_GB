using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Booking.Consumers;
using Restaurant.Booking.Saga;

namespace Restaurant.Booking.Extensions
{
    internal static class ServicesEx
    {
        /// <summary> Adding and configuring mass transit. </summary>
        /// <param name="services">service collection interface</param>
        internal static void AddAndConfigMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<BookingRequestConsumer>(configurator =>
                    {
                        configurator.UseScheduledRedelivery(config =>
                        {
                            config.Intervals(TimeSpan.FromSeconds(10),
                                TimeSpan.FromSeconds(20),
                                TimeSpan.FromSeconds(30));
                        });
                        configurator.UseMessageRetry(config =>
                        {
                            config.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                                intervalIncrement: TimeSpan.FromSeconds(2));
                        });
                    })
                    .Endpoint(cfg => cfg.Temporary = true);

                configure.AddConsumer<KitchenAccidentConsumer>(configurator =>
                    {
                        configurator.UseScheduledRedelivery(config =>
                        {
                            config.Intervals(TimeSpan.FromSeconds(10),
                                TimeSpan.FromSeconds(20),
                                TimeSpan.FromSeconds(30));
                        });
                        configurator.UseMessageRetry(config =>
                        {
                            config.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                                intervalIncrement: TimeSpan.FromSeconds(2));
                        });
                    })
                    .Endpoint(cfg => cfg.Temporary = true);

                configure.AddConsumer<BookingRequestFaultConsumer>(configurator =>
                    {
                        configurator.UseScheduledRedelivery(config =>
                        {
                            config.Intervals(TimeSpan.FromSeconds(10),
                                TimeSpan.FromSeconds(20),
                                TimeSpan.FromSeconds(30));
                        });
                        configurator.UseMessageRetry(config =>
                        {
                            config.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                                intervalIncrement: TimeSpan.FromSeconds(2));
                        });
                    })
                    .Endpoint(cfg => cfg.Temporary = true);

                configure.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>()
                    .Endpoint(cfg => cfg.Temporary = true)
                    .InMemoryRepository();

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
