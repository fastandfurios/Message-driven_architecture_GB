namespace Restaurant.Booking.Consumers.Expires.Interfaces;

public interface ITimeArrivalGuestExpire
{
    Guid OrderId { get; }
}