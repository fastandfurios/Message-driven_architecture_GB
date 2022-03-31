#region references
using System.Text;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Booking;
using Restaurant.Booking.Consumers;
using Restaurant.Booking.Saga;
#endregion

#region main
Console.OutputEncoding = Encoding.UTF8;
CreateHostBuilder(args).Build().Run();
#endregion

#region methods
static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<BookingRequestConsumer>()
                    .Endpoint(cfg => cfg.Temporary = true);

                configure.AddConsumer<KitchenAccidentConsumer>()
                    .Endpoint(cfg => cfg.Temporary = true);

                configure.AddConsumer<BookingRequestFaultConsumer>()
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

            services.AddTransient<RestaurantBooking>();

            services.AddTransient<RestaurantBookingSaga>();

            services.AddTransient<Restaurant.Booking.Restaurant>();

            services.AddHostedService<Worker>();
        });
#endregion