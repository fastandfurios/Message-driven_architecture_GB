using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging
{
    public class Consumer : IDisposable
    {
        //private readonly string _queueName;
        private readonly string _hostName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        //public Consumer(string queueName, string hostName)
        //{
        //    _queueName = queueName;
        //    _hostName = hostName;
        //    var factory = new ConnectionFactory { HostName = _hostName };
        //    _connection = factory.CreateConnection();
        //    _channel = _connection.CreateModel();
        //}

        public Consumer(string hostName)
        {
            _hostName = hostName;
            var factory = new ConnectionFactory { HostName = _hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
        {
            var queueName = _channel.QueueDeclare().QueueName;

            //_channel.ExchangeDeclare(exchange: "direct_exchange", type: "direct");

            _channel.ExchangeDeclare(exchange: "fanout_exchange", type: ExchangeType.Fanout);

            //_channel.QueueDeclare(queue: _queueName,
            //    durable: true,
            //    exclusive: false,
            //    autoDelete: false,
            //    arguments: null);

            //_channel.QueueBind(queue: _queueName,
            //    exchange: "direct_exchange",
            //    routingKey: _queueName);

            _channel.QueueBind(queue: queueName,
                exchange: "fanout_exchange",
                routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += receiveCallback;

            //_channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}