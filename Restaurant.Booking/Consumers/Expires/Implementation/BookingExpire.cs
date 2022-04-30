using Restaurant.Booking.Consumers.Expires.Interfaces;
using Restaurant.Booking.Saga;

namespace Restaurant.Booking.Consumers.Expires.Implementation
{
    public class BookingExpire : IBookingExpire
    {
        private readonly RestaurantBooking _instance;

        public BookingExpire(RestaurantBooking instance)
        {
            _instance = instance;
        }

        public Guid OrderId => _instance.OrderId;
    }
}
