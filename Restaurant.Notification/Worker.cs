using System.Text;
using Messaging.Consumers;
using Messaging.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Restaurant.Notification
{
    public class Worker : BackgroundService
    {
        private readonly IConsumer _consumer;

        public Worker()
        {
            _consumer = new ConsumerFanout("localhost");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                _consumer.Receive((sender, args) =>
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("[x] Received {0}", message);
                });
            }, stoppingToken).ConfigureAwait(true);
        }
    }
}
