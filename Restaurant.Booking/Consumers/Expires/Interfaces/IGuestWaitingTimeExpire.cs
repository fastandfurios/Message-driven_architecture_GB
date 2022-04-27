namespace Restaurant.Booking.Consumers.Expires.Interfaces;

public interface IGuestWaitingTimeExpire
{
    Guid OrderId { get; }
}