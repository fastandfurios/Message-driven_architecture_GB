using Restaurant.Messages.Interfaces;

namespace Restaurant.Messages.Implementation
{
    public class Notify : INotify
    {
        public Notify(Guid clientId, Guid orderId, string message)
        {
            ClientId = clientId;
            OrderId = orderId;
            Message = message;
        }

        public Guid ClientId { get; }
        public Guid OrderId { get; }
        public string Message { get; }
    }
}
