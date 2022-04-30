using MassTransit;
using Microsoft.Extensions.Logging;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Notification.Consumers
{
    public class NotifyFaultConsumer : IConsumer<Fault<INotify>>
    {
        private readonly ILogger<NotifyFaultConsumer> _logger;

        public NotifyFaultConsumer(ILogger<NotifyFaultConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Fault<INotify>> context)
        {
            _logger.Log(LogLevel.Warning, $"Ошибка в уведомлении заказа [{context.Message.Message.OrderId}]");
            return context.ConsumeCompleted;
        }
    }
}
