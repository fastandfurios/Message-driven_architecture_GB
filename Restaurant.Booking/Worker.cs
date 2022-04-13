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
                for (int i = 1; i <= 4; i++)
                {
                    await Task.Delay(millisecondsDelay: 20000, stoppingToken);

                    Console.WriteLine("Привет! Желаете забронировать столик?");

                    await _bus.Publish(new BookingRequest(NewId.NextGuid(), NewId.NextGuid(), new Dish { Id = GetId(i) }, TimeSpan.FromSeconds(Random.Shared.Next(7, 15)), DateTime.Now),
                        stoppingToken);
                }
            }
        }

        private int GetId(int i)
        {
            var number = 6;

            if (i < 4)
                return Random.Shared.Next(1, 5);

            return number;
        }
    }
}
