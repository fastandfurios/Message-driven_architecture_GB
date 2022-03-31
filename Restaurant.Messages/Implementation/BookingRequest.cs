using Restaurant.Messages.Interfaces;

namespace Restaurant.Messages.Implementation
{
    public class BookingRequest : IBookingRequest
    {
        public BookingRequest(Guid clientId, DateTime dateTime, Guid orderId, Dish? preOrder)
        {
            ClientId = clientId;
            DateTime = dateTime;
            OrderId = orderId;
            PreOrder = preOrder;
        }

        public Guid ClientId { get; }
        public DateTime DateTime { get; }
        public Guid OrderId { get; }
        public Dish? PreOrder { get; }
    }
}
