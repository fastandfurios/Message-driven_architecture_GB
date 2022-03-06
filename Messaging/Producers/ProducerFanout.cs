using System.Text;
using Messaging.Interfaces;
using RabbitMQ.Client;

namespace Messaging.Producers
{
    public class ProducerFanout : IProducer
    {
        public ProducerFanout(string hostName)
        {
            HostName = hostName;
        }

        public string HostName { get; set; }

        public void Send(string message)
        {
            var factory = new ConnectionFactory { HostName = HostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "fanout_exchange",
                type: ExchangeType.Fanout,
                durable: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "fanout_exchange",
                routingKey: "",
                basicProperties: null,
                body: body);
        }
    }
}
