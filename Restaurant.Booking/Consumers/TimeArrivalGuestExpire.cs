﻿using Restaurant.Booking.Saga;

namespace Restaurant.Booking.Consumers
{
    public class TimeArrivalGuestExpire : ITimeArrivalGuestExpire
    {
        private readonly RestaurantBooking _instance;

        public TimeArrivalGuestExpire(RestaurantBooking instance)
        {
            _instance = instance;
        }

        public Guid OrderId => _instance.OrderId;
    }
}
