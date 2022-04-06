#region references
using System.Text;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Notification;
using Restaurant.Notification.Consumers;
#endregion

#region main
Console.OutputEncoding = Encoding.UTF8;
CreateHostBuilder(args).Build().Run();
#endregion

#region methods
static IHostBuilder CreateHostBuilder(string[] args)
=> Host.CreateDefaultBuilder(args)
        .ConfigureServices((services) =>
        {
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<NotifyConsumer>()
                    .Endpoint(configure => configure.Temporary = true);

                cfg.UsingRabbitMq((context, config) =>
                {
                    config.ConfigureEndpoints(registration: context);
                });
            });

            services.AddSingleton<Notifier>();
        });
#endregion