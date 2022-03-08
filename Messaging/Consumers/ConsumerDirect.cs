using Messaging.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging.Consumers
{
    public class ConsumerDirect : IConsumer
    {
        private readonly string _queueName;
        private readonly string _hostName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public ConsumerDirect(string queueName, string hostName)
        {
            _queueName = queueName;
            _hostName = hostName;
            var factory = new ConnectionFactory { HostName = _hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        
        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
        {
            _channel.ExchangeDeclare(exchange: "direct_exchange", type: "direct");

            _channel.QueueDeclare(queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(queue: _queueName,
                exchange: "direct_exchange",
                routingKey: _queueName);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += receiveCallback;

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}