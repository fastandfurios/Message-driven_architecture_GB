using MassTransit;
using Microsoft.Extensions.Logging;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Booking.Consumers
{
    public class KitchenAccidentConsumer : IConsumer<IKitchenAccident>
    {
        private readonly Restaurant _restaurant;
        private readonly ILogger<KitchenAccidentConsumer> _logger;

        public KitchenAccidentConsumer(Restaurant restaurant, ILogger<KitchenAccidentConsumer> logger)
        {
            _restaurant = restaurant;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IKitchenAccident> context)
        {
            _logger.Log(LogLevel.Warning, $"[OrderId {context.Message.OrderId}] Отмена в зале");
            _restaurant.CancelReservationAsync();
            
            return context.ConsumeCompleted;
        }
    }
}
