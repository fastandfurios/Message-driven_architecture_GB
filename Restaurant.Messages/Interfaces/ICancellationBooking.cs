namespace Restaurant.Messages.Interfaces
{
    public interface ICancellationBooking
    {
        Dish Dish { get; }
        Guid OrderId { get; }
    }
}
