using System.Text;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Messages;
using Restaurant.Messages.Implementation;

namespace Restaurant.Booking
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;

        public Worker(IBus bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = Encoding.UTF8;
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(millisecondsDelay: 5000, stoppingToken);
                Console.WriteLine("Привет! Желаете забронировать столик?");

                await _bus.Publish(new BookingRequest(NewId.NextGuid(), NewId.NextGuid(), new Dish { Id = Random.Shared.Next(1, 5) }), 
                    stoppingToken);
            }
        }
    }
}
