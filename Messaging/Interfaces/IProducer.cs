namespace Messaging.Interfaces
{
    public interface IProducer
    {
        string HostName { get; set; }
        void Send(string message);
    }
}
