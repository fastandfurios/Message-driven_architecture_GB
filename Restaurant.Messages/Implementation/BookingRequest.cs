using Restaurant.Messages.Interfaces;

namespace Restaurant.Messages.Implementation
{
    public class BookingRequest : IBookingRequest
    {
        public BookingRequest(Guid clientId, Guid orderId, Dish? preOrder, TimeSpan arrivalTime)
        {
            ClientId = clientId;
            OrderId = orderId;
            PreOrder = preOrder;
            ArrivalTime = arrivalTime;
        }

        public Guid ClientId { get; }
        public Guid OrderId { get; }
        public Dish? PreOrder { get; }
        public TimeSpan ArrivalTime { get; }
    }
}
