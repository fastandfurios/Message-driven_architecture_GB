using System.Text;
using RabbitMQ.Client;

namespace Messaging
{
    public class Producer
    {
        private readonly string _hostName;
        //private readonly string _queueName;

        //public Producer(string hostName, string queueName)
        //{
        //    _hostName = hostName;
        //    _queueName = queueName;
        //}

        public Producer(string hostName)
        {
            _hostName = hostName;
        }

        public void Send(string message)
        {
            var factory = new ConnectionFactory { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //channel.ExchangeDeclare(exchange: "direct_exchange",
            //    type: "direct",
            //    durable: false,
            //    autoDelete: false,
            //    arguments: null);

            channel.ExchangeDeclare(exchange: "fanout_exchange",
                type: ExchangeType.Fanout,
                durable: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            //channel.BasicPublish(exchange: "direct_exchange",
            //    routingKey: _queueName,
            //    basicProperties: null,
            //    body: body);

            channel.BasicPublish(exchange: "fanout_exchange",
                routingKey: "",
                basicProperties: null,
                body: body);
        }
    }
}
