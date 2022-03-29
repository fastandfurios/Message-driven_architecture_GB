using System.Text;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Messaging;

namespace Restaurant.Booking
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly Restaurant _restaurant;

        public Worker(IBus bus, Restaurant restaurant)
        {
            _bus = bus;
            _restaurant = restaurant;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = Encoding.UTF8;
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(millisecondsDelay: 10000, stoppingToken);
                Console.WriteLine("Привет! Желаете забронировать столик?");
                var result = await _restaurant.BookFreeTableAsync(1, stoppingToken);
                await _bus.Publish(new TableBooked(NewId.NextGuid(), result ?? false, NewId.NextGuid(), new Dish{ Id = Random.Shared.Next(1, 5) }),
                    context => context.Durable = false, stoppingToken);
            }
        }
    }
}
