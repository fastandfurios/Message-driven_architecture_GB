namespace Restaurant.Messaging
{
    public interface ICancellationBooking
    {
        public Guid OrderId { get; }
        public Dish? Dish { get; }
    }
}
