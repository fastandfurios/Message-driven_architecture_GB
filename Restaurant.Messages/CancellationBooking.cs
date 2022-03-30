namespace Restaurant.Messaging
{
    public class CancellationBooking : ICancellationBooking
    {
        public CancellationBooking(Guid orderId, Dish? dish)
        {
            OrderId = orderId;
            Dish = dish;
        }

        public Guid OrderId { get; }
        public Dish? Dish { get; }
    }
}
