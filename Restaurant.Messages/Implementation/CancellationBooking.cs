using Restaurant.Messages.Interfaces;

namespace Restaurant.Messages.Implementation
{
    public class CancellationBooking : ICancellationBooking
    {
        public CancellationBooking(Guid orderId, Guid clientId, string message, Dish? dish = default)
        {
            OrderId = orderId;
            ClientId = clientId;
            Message = message;
            Dish = dish;
        }

        public Guid ClientId { get; }
        public string Message { get; }
        public Guid OrderId { get; }
        public Dish? Dish { get; }
    }
}
