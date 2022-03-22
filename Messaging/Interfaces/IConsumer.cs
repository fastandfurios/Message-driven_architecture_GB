using RabbitMQ.Client.Events;

namespace Messaging.Interfaces
{
    public interface IConsumer : IDisposable
    {
        void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback);
    }
}
