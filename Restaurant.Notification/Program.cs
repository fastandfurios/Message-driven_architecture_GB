#region references
using System.Text;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Notification;
#endregion

Console.OutputEncoding = Encoding.UTF8;
CreateHostBuilder(args).Build().Run();

#region methods
static IHostBuilder CreateHostBuilder(string[] args)
=> Host.CreateDefaultBuilder(args)
        .ConfigureServices((services) =>
        {
            services.AddMassTransit(cofig =>
            {
                cofig.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(c =>
                    {
                        c.Exponential(retryLimit: 5,
                            minInterval: TimeSpan.FromSeconds(1),
                            maxInterval: TimeSpan.FromSeconds(100),
                            intervalDelta: TimeSpan.FromSeconds(5));

                        c.Ignore<StackOverflowException>();
                        c.Ignore<ArgumentNullException>(filter => filter.Message.Contains("Consumer"));
                    });

                    cfg.ConfigureEndpoints(registration: context);
                });
            });

            services.AddSingleton<Notifier>();
        });
#endregion