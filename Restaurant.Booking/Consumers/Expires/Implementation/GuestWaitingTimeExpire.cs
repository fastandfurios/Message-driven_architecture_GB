using Restaurant.Booking.Consumers.Expires.Interfaces;
using Restaurant.Booking.Saga;

namespace Restaurant.Booking.Consumers.Expires.Implementation
{
    public class GuestWaitingTimeExpire : IGuestWaitingTimeExpire
    {
        private readonly RestaurantBooking _instance;

        public GuestWaitingTimeExpire(RestaurantBooking instance)
        {
            _instance = instance;
        }

        public Guid OrderId => _instance.OrderId;
    }
}
