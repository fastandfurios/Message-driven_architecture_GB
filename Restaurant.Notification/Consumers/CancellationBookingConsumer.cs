using MassTransit;
using Restaurant.Messages;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Notification.Consumers
{
    public class CancellationBookingConsumer : IConsumer<ICancellationBooking>
    {
        private readonly Notifier _notifier;

        public CancellationBookingConsumer(Notifier notifier)
        {
            _notifier = notifier;
        }

        public Task Consume(ConsumeContext<ICancellationBooking> context)
        {
            _notifier.Notify(context.Message.OrderId, context.Message.ClientId, context.Message.Message);

            return context.ConsumeCompleted;
        }
    }
}
