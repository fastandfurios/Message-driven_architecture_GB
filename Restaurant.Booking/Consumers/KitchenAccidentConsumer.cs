using MassTransit;
using Restaurant.Messaging;

namespace Restaurant.Booking.Consumers
{
    public class KitchenAccidentConsumer : IConsumer<IKitchenAccident>
    {
        private readonly Restaurant _restaurant;
        private readonly Manager _manager;

        public KitchenAccidentConsumer(Restaurant restaurant, Manager manager)
        {
            _restaurant = restaurant;
            _manager = manager;
        }

        public Task Consume(ConsumeContext<IKitchenAccident> context)
        {
            _restaurant.CancelReservationAsync();
            _manager.SendNotification(context.Message.OrderId, context.Message.Dish);

            return context.ConsumeCompleted;
        }
    }
}
