namespace Restaurant.Booking.Consumers.Expires.Interfaces
{
    public interface IBookingExpire
    {
        Guid OrderId { get; }
    }
}
