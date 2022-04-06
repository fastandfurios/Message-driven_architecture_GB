namespace Restaurant.Booking.Consumers;

public interface ITimeArrivalGuestExpire
{
    Guid OrderId { get; }
}