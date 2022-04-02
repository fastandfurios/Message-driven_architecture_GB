using Restaurant.Messages.Interfaces;

namespace Restaurant.Messages.Implementation
{
    public class CancellationBooking : ICancellationBooking
    {
        public CancellationBooking(Guid orderId, Dish? dish = default)
        {
            OrderId = orderId;
            Dish = dish;
        }
        
        public Guid OrderId { get; }
        public Dish? Dish { get; }
    }
}
