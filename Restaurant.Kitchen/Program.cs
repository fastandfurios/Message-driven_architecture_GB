#region references
using System.Text;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Kitchen;
using Restaurant.Kitchen.Consumers;
#endregion

#region main
Console.OutputEncoding = Encoding.UTF8;
CreateHostBuilder(args).Build().Run();
#endregion

#region methods

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<KitchenTableBookedConsumer>();

                configure.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddSingleton<Manager>();
        });
#endregion
