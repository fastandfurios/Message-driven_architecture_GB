#region references
using System.Text;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Booking;
using Restaurant.Booking.Consumers;
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
                configure.AddConsumer<KitchenAccidentConsumer>();

                configure.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            //Обратите внимание, что для MassTransit V8 или более поздней версии этот пакет больше не требуется и на него не следует ссылаться.‎
            //services.AddMassTransitHostedService(waitUntilStarted: true);

            services.AddTransient<Restaurant.Booking.Restaurant>();

            services.AddHostedService<Worker>();

            services.AddSingleton<Manager>();
        });
#endregion