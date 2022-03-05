#region references
using System.Text;
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
            services.AddHostedService<Worker>();
        });
#endregion