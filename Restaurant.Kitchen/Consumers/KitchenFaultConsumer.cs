using MassTransit;
using Microsoft.Extensions.Logging;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Kitchen.Consumers
{
    public class KitchenFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        private readonly ILogger<KitchenFaultConsumer> _logger;

        public KitchenFaultConsumer(ILogger<KitchenFaultConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            _logger.Log(LogLevel.Warning, $"Отмена приготовления заказа {context.Message.Message.OrderId} на кухне");
            
            return context.ConsumeCompleted;
        }
    }
}
