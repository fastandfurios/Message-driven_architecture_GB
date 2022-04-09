using MassTransit;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Notification.Consumers
{
    public class NotifyFaultConsumer : IConsumer<Fault<INotify>>
    {
        public Task Consume(ConsumeContext<Fault<INotify>> context)
        {
            return context.ConsumeCompleted;
        }
    }
}
