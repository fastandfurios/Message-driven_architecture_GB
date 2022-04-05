namespace Restaurant.Booking.Consumers;

public interface IGuestWaitingTimeExpire
{
    Guid OrderId { get; }
}