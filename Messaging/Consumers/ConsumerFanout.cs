using Messaging.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging.Consumers
{
    public class ConsumerFanout : IConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _hostName;

        public ConsumerFanout(string hostName)
        {
            _hostName = hostName;
            var factory = new ConnectionFactory { HostName = _hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
        {
            var queueName = _channel.QueueDeclare().QueueName;

            _channel.ExchangeDeclare(exchange: "fanout_exchange", type: ExchangeType.Fanout);

            _channel.QueueBind(queue: queueName,
                exchange: "fanout_exchange",
                routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += receiveCallback;

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}
