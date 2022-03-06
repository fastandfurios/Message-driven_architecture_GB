using RabbitMQ.Client.Events;

namespace Messaging.Interfaces
{
    public interface IConsumer : IDisposable
    {
        string HostName { get; set; }
        void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback);
    }
}
