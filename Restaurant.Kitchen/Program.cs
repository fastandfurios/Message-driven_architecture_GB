#region references
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Kitchen;
using Restaurant.Kitchen.DAL.Models;
using Restaurant.Kitchen.DAL.Repositories.Implementation;
using Restaurant.Kitchen.DAL.Repositories.Interfaces;
using Restaurant.Kitchen.Extensions;
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
            services.AddAndConfigMassTransit();

            services.AddSingleton<Manager>();

            services.AddSingleton<IKitchenMessageRepository<KitchenTableBookedModel>, KitchenMessageRepository>();

            services.AddSingleton<IConnection, Connection>();

            services.SqLiteConfiguring("Data Source=C:\\Test\\Messages.db;Version=3;New=True;Compress=True;");
        });
#endregion
