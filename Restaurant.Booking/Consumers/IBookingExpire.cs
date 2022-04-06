namespace Restaurant.Booking.Consumers
{
    public interface IBookingExpire
    {
        Guid OrderId { get; }
    }
}
