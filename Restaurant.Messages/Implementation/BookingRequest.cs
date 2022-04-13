using Restaurant.Messages.Interfaces;

namespace Restaurant.Messages.Implementation
{
    public class BookingRequest : IBookingRequest
    {
        public BookingRequest(Guid clientId, Guid orderId, Dish? preOrder, TimeSpan arrivalTime, DateTime creationDate)
        {
            ClientId = clientId;
            OrderId = orderId;
            PreOrder = preOrder;
            ArrivalTime = arrivalTime;
            CreationDate = creationDate;
        }

        public Guid ClientId { get; }
        public Guid OrderId { get; }
        public Dish? PreOrder { get; }
        public TimeSpan ArrivalTime { get; }
        public DateTime CreationDate { get; }
    }
}
