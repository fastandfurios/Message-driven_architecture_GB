﻿using MassTransit;
using Restaurant.Messaging;

namespace Restaurant.Booking.Consumers
{
    public class KitchenAccidentConsumer : IConsumer<IKitchenAccident>
    {
        private readonly Restaurant _restaurant;

        public KitchenAccidentConsumer(Restaurant restaurant)
        {
            _restaurant = restaurant;
        }

        public Task Consume(ConsumeContext<IKitchenAccident> context)
        {
            _restaurant.CancelReservationAsync();
            context.Publish(new CancellationBooking(context.Message.OrderId, context.Message.Dish));

            return context.ConsumeCompleted;
        }
    }
}
