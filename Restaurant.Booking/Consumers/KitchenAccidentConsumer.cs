using MassTransit;
using Restaurant.Messages.Implementation;
using Restaurant.Messages.Interfaces;

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
            context.Publish(new KitchenAccident(context.Message.OrderId, context.Message.Dish));

            return context.ConsumeCompleted;
        }
    }
}
