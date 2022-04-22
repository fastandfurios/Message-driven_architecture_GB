using MassTransit;
using MassTransit.Audit;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Booking.Consumers;
using Restaurant.Booking.Saga;
using Restaurant.Messages.Implementation;

namespace Restaurant.Booking.Extensions
{
    internal static class ServicesEx
    {
        /// <summary> Adding and configuring mass transit. </summary>
        /// <param name="services">service collection interface</param>
        internal static void AddAndConfigMassTransit(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var auditStore = serviceProvider.GetService<IMessageAuditStore>();

            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<BookingRequestConsumer>(configurator =>
                    {
                        configurator.UseScheduledRedelivery(config =>
                        {
                            config.Interval(1, TimeSpan.FromSeconds(2));
                        });
                        configurator.UseMessageRetry(config =>
                        {
                            config.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                                intervalIncrement: TimeSpan.FromSeconds(2));
                        });
                    });

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
                    });
                configure.AddConsumer<BookingRequestFaultConsumer>();

                configure.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>()
                    .InMemoryRepository();

                configure.AddDelayedMessageScheduler();

                configure.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseDelayedMessageScheduler();
                    cfg.UseInMemoryOutbox();
                    cfg.ConfigureEndpoints(context);
                    cfg.ConnectSendAuditObservers(auditStore);
                    cfg.ConnectConsumeAuditObserver(auditStore);
                });
            });
        }
    }
}
