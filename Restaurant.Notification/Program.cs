#region references
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Notification;
using Restaurant.Notification.Extensions;
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
            services.AddAndConfigMassTransit();

            services.AddSingleton<Notifier>();
        });
#endregion