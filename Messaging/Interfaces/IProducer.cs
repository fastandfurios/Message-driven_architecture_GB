namespace Messaging.Interfaces
{
    public interface IProducer
    {
        void Send(string message);
    }
}
