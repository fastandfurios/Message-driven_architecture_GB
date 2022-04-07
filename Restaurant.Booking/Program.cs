#region references
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Booking;
using Restaurant.Booking.Extensions;
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
            services.AddAndConfigMassTransit();

            services.AddTransient<RestaurantBooking>();

            services.AddTransient<RestaurantBookingSaga>();

            services.AddTransient<Restaurant.Booking.Restaurant>();

            services.AddHostedService<Worker>();
        });
#endregion