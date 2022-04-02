using Restaurant.Messages.Interfaces;

namespace Restaurant.Messages.Implementation
{
    public class BookingRequest : IBookingRequest
    {
        public BookingRequest(Guid clientId, Guid orderId, Dish? preOrder)
        {
            ClientId = clientId;
            OrderId = orderId;
            PreOrder = preOrder;
        }

        public Guid ClientId { get; }
        public Guid OrderId { get; }
        public Dish? PreOrder { get; }
    }
}
